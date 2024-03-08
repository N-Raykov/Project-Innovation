using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

	[SerializeField] private Vector3[] points;

	[SerializeField] private BezierControlPointMode[] modes;

	[SerializeField] private bool loop;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}

	public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetControlPoint(int index) {
		return points[index];
	}

	public void SetControlPoint(int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Length - 2] += delta;
					points[points.Length - 1] = point;
				}
				else if (index == points.Length - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Length) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public float GetLength()
	{
		float length = 0f;
		int curveCount = CurveCount;
		for (int i = 0; i < curveCount; i++)
		{
			length += GetCurveLength(i);
		}
		return length;
	}

	private float GetCurveLength(int curveIndex)
	{
		int startIndex = curveIndex * 3;
		Vector3 p0 = points[startIndex];
		Vector3 p1 = points[startIndex + 1];
		Vector3 p2 = points[startIndex + 2];
		Vector3 p3 = points[startIndex + 3];

		return BezierCurveLength(p0, p1, p2, p3);
	}

	private float BezierCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float length = 0f;
		Vector3 lastPoint = p0;
		int segments = 10;
		for (int i = 1; i <= segments; i++)
		{
			float t = i / (float)segments;
			Vector3 point = Bezier.GetPoint(p0, p1, p2, p3, t);
			length += Vector3.Distance(lastPoint, point);
			lastPoint = point;
		}
		return length;
	}

	public void RemoveControlPoint(int index)
	{
		if (index % 3 != 0 || ControlPointCount <= 4)
		{
			Debug.LogWarning("Cannot remove control point. The spline must have at least 4 control points.");
			return;
		}

		Array.Copy(points, index + 3, points, index, points.Length - index - 3);
		Array.Resize(ref points, points.Length - 3);

		Array.Copy(modes, (index + 1) / 3 + 1, modes, (index + 1) / 3, modes.Length - ((index + 1) / 3 + 1));
		Array.Resize(ref modes, modes.Length - 1);

		for (int i = (index + 1) / 3; i < modes.Length; i++)
		{
			if (modes[i] != BezierControlPointMode.Free)
			{
				modes[i] = BezierControlPointMode.Free;
			}
		}
	}

	public void SplitSpline(int index)
	{
		if (index < 0 || index >= ControlPointCount)
		{
			Debug.LogWarning("Invalid index for splitting spline.");
			return;
		}

		if (index % 3 != 0)
		{
			Debug.LogWarning("Cannot split control point. Choose the center control point instead. You will thank me.");
			return;
		}

		SplineNetwork splineNetwork = GetComponentInParent<SplineNetwork>();

		GameObject branchObject = new GameObject("BranchSpline");
		branchObject.transform.SetParent(this.transform.parent);
		branchObject.transform.position = points[index];
		BezierSpline branchSpline = branchObject.AddComponent<BezierSpline>();
		LineRenderer linerenderer = branchObject.AddComponent<LineRenderer>();

		if (splineNetwork != null)
		{
			splineNetwork.AddSplitPoint(index, branchSpline, this);
		}

		if (index > 0)
		{
			SetControlPointMode(index, BezierControlPointMode.Split);
			EnforceMode(index - 1);
		}
	}

	public void RemoveSplit(int index)
	{
		SplineNetwork splineNetwork = GetComponentInParent<SplineNetwork>();
		if (splineNetwork != null && splineNetwork.RemoveSplitPoint(index) == true && index > 0)
		{
			SetControlPointMode(index, BezierControlPointMode.Free);
			EnforceMode(index - 1);
		}
	}

	public void ConnectSplines(BezierSpline main, BezierSpline branch)
	{
		SplineNetwork.Instance.AddConnection(main, branch);
	}

	public void DisconnectSplines(BezierSpline connection)
	{
		SplineNetwork.Instance.DisconnectSplines(this, connection);
	}

	public BezierControlPointMode GetControlPointMode(int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode(int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	public void EnforceMode(int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Length - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Length) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Length) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Length - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		if (mode == BezierControlPointMode.Split)
		{
			enforcedTangent = points[enforcedIndex] - middle;
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

	public Vector3 GetPoint(float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}

	public Vector3 PointOnTrack(Vector3 point, out float progress)
	{
		progress = 0f;
		float shortestDistance = 10000f;
		Vector3 closestPoint = GetPoint(0f);
		for (float t = 0; t <= 1; t += 0.001f)
		{
			Vector3 splinePoint = GetPoint(t);
			float distance = Vector3.Distance(splinePoint, point);
			if (distance <= shortestDistance)
			{
				shortestDistance = distance;
				closestPoint = splinePoint;
				progress = t;
			}

		}

		return closestPoint;
	}

	public Vector3 PointOnTrack(Vector3 point)
	{
		float progress;
		return PointOnTrack(point, out progress);
	}

	public float GetTime(Vector3 point)
	{
		float step = 0.01f;
		float closestTime = 0f;
		float closestDistance = Mathf.Infinity;

		for (float t = 0; t <= 1; t += step)
		{
			Vector3 splinePoint = GetPoint(t);
			float distance = Vector3.Distance(splinePoint, point);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestTime = t;
			}
		}

		return closestTime;
	}

	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;

		Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(points.Length - 4);

		if (loop) {
			points[points.Length - 1] = points[0];
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}

	public void Reset () {
		points = new Vector3[] {
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f)
		};
		modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}
}