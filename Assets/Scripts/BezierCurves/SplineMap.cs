using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SplineMap : MonoBehaviour
{
    [SerializeField] int mapPrecision = 10;
    private List<GameObject> children = new List<GameObject>();

    void Start()
    {
        PopulateChildren();
        ProjectTrack();
    }

    void PopulateChildren()
    {
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
    }

    void ProjectTrack()
    {
        foreach (GameObject child in children)
        {
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            BezierSpline spline = child.GetComponent<BezierSpline>();

            Vector3 point = spline.GetPoint(0f);
            int steps = mapPrecision * spline.CurveCount;

            lineRenderer.positionCount = steps + 1;

            for (int i = 0; i <= steps; i++)
            {
                point = spline.GetPoint(i / (float)steps);
                lineRenderer.SetPosition(i, new Vector3(point.x, 4, point.z));
            }
        }
    }
}
