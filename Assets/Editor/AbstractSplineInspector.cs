using UnityEditor;
using UnityEngine;

public abstract class AbstractSplineInspector : Editor
{
	protected const int stepsPerCurve = 10;
	protected const float directionScale = 0.2f;
	protected const float handleSize = 0.04f;
	protected const float pickSize = 0.06f;

	protected static Color[] modeColors = {
		Color.white,
		Color.yellow,
		Color.cyan,
		Color.red
	};

	protected BezierSpline spline;
	protected Transform handleTransform;
	protected Quaternion handleRotation;
	protected int selectedIndex = -1;

	protected virtual void ShowDirections(BezierSpline splineInstance)
	{
		Handles.color = Color.green;
		Vector3 point = splineInstance.GetPoint(0f);
		Handles.DrawLine(point, point + splineInstance.GetVelocity(0f) * directionScale);
		int steps = stepsPerCurve * splineInstance.CurveCount;
		for (int i = 1; i <= steps; i++)
		{
			point = splineInstance.GetPoint(i / (float)steps);
			Handles.DrawLine(point, point + splineInstance.GetVelocity(i / (float)steps) * directionScale);
		}
	}

	protected virtual Vector3 ShowPoint(int index, BezierSpline splineInstance)
	{
		Vector3 point = handleTransform.TransformPoint(splineInstance.GetControlPoint(index));
		float size = HandleUtility.GetHandleSize(point);
		if (index == 0)
		{
			size *= 2f;
		}
		Handles.color = modeColors[(int)splineInstance.GetControlPointMode(index)];
		if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
		{
			selectedIndex = index;
			Repaint();
		}
		if (selectedIndex == index)
		{
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(splineInstance, "Move Point");
				EditorUtility.SetDirty(splineInstance);
				splineInstance.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
			}
		}
		return point;
	}
}
