using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : AbstractSplineInspector {

	[SerializeField] GameObject selectedSpline;

	public override void OnInspectorGUI () {
		spline = target as BezierSpline;
		EditorGUI.BeginChangeCheck();
		bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Toggle Loop");
			EditorUtility.SetDirty(spline);
			spline.Loop = loop;
		}
		if (GUILayout.Button("Add Curve")) {
			Undo.RecordObject(spline, "Add Curve");
			spline.AddCurve();
			EditorUtility.SetDirty(spline);
		}

		EditorGUI.BeginChangeCheck();
		GameObject connectSplineObject;
		BezierSpline connectedSpline;
		
		if (SplineNetwork.Instance.TryGetConnectedSpline(spline, out connectedSpline))
        {
			connectSplineObject = (GameObject)EditorGUILayout.ObjectField("Connect", connectedSpline.gameObject, typeof(GameObject), true);
		}
        else
        {
			connectSplineObject = (GameObject)EditorGUILayout.ObjectField("Connect", selectedSpline, typeof(GameObject), true);
		}

		if (EditorGUI.EndChangeCheck() && connectSplineObject != null && connectSplineObject != spline.gameObject)
		{
			selectedSpline = connectSplineObject;
			BezierSpline connectSpline = selectedSpline.GetComponent<BezierSpline>();
			if (connectSpline == null)
			{
				Debug.LogWarning("The selected GameObject does not contain a BezierSpline component.");
				return;
			}
			Undo.RecordObject(spline, "Connect Splines");
			spline.ConnectSplines(spline, connectSpline);
			EditorUtility.SetDirty(spline);
		}

		if (GUILayout.Button("Disconnect"))
		{
			if (connectSplineObject == null)
			{
				Debug.LogWarning("The selected GameObject does not contain a BezierSpline component.");
				return;
			}
			Undo.RecordObject(spline, "Disconnect");
			spline.DisconnectSplines(connectSplineObject.GetComponent<BezierSpline>());
			selectedSpline = null;
			EditorUtility.SetDirty(spline);
		}

		if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount) {
			DrawSelectedPointInspector();
		}
	}

	private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		GUILayout.Label("Index: " + selectedIndex.ToString());
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Move Point");
			EditorUtility.SetDirty(spline);
			spline.SetControlPoint(selectedIndex, point);
		}

		EditorGUI.BeginChangeCheck();
		BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Change Point Mode");
			spline.SetControlPointMode(selectedIndex, mode);
			EditorUtility.SetDirty(spline);
		}


		if (mode != BezierControlPointMode.Split)
		{
			if (GUILayout.Button("Split Spline"))
			{
				Undo.RecordObject(spline, "Split Spline");
				spline.SplitSpline(selectedIndex);
				EditorUtility.SetDirty(spline);
			}
		}
		else
		{
			if (GUILayout.Button("Remove Split"))
			{
				Undo.RecordObject(spline, "Remove Split");
				spline.RemoveSplit(selectedIndex);
				EditorUtility.SetDirty(spline);
			}
		}

		if (GUILayout.Button("Remove Point"))
		{
			Undo.RecordObject(spline, "Remove Point");
			spline.RemoveControlPoint(selectedIndex);
			EditorUtility.SetDirty(spline);
			selectedIndex = -1; 
		}
	}

	private void OnSceneGUI()
	{
		Object targetObject = target;
		BezierSpline[] splines;
		if (targetObject is GameObject)
		{
			splines = ((GameObject)targetObject).GetComponentsInChildren<BezierSpline>();
		}
        else
        {
            splines = ((BezierSpline)targetObject).gameObject.GetComponentsInChildren<BezierSpline>();
        }

		foreach (BezierSpline spline in splines)
		{
			handleTransform = spline.transform;
			handleRotation = Tools.pivotRotation == PivotRotation.Local ?
				handleTransform.rotation : Quaternion.identity;

			Vector3 p0 = ShowPoint(0, spline);
			for (int i = 1; i < spline.ControlPointCount; i += 3)
			{
				Vector3 p1 = ShowPoint(i, spline);
				Vector3 p2 = ShowPoint(i + 1, spline);
				Vector3 p3 = ShowPoint(i + 2, spline);

				Handles.color = Color.gray;
				Handles.DrawLine(p0, p1);
				Handles.DrawLine(p2, p3);

				Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
				p0 = p3;
			}
			ShowDirections(spline);
		}
	}
}