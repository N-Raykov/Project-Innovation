using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SplineMap : MonoBehaviour
{
    [SerializeField] int mapPrecision = 10;
    [SerializeField] GameObject splineMap;
    void Start()
    {
        ProjectTrack();
    }

    void ProjectTrack()
    {
        LineRenderer lineRenderer = splineMap.GetComponent<LineRenderer>();
        BezierSpline spline = splineMap.GetComponent<BezierSpline>();

        Vector3 point = spline.GetPoint(0f);
        int steps = mapPrecision * spline.CurveCount;

        lineRenderer.positionCount = steps + 1;

        for (int i = 0; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            lineRenderer.SetPosition(i, new Vector3(point.x, -44, point.z));
        }
    }
}
