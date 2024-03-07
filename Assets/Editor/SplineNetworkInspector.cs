using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineNetwork))]
public class ParentInspector : AbstractSplineInspector
{
	private void OnSceneGUI()
	{
		MonoBehaviour targetBehaviour = target as MonoBehaviour;
		if (targetBehaviour != null)
		{
			GameObject targetObject = targetBehaviour.gameObject;
			BezierSpline[] splines = targetObject.GetComponentsInChildren<BezierSpline>();

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
}
