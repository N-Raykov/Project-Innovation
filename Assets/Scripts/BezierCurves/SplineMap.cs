using UnityEngine;
using UnityEngine.UI;

public class SplineMap : MonoBehaviour
{
    [SerializeField] BezierSpline spline;
    [SerializeField] int mapPrecision = 10;
    
    private LineRenderer lineRenderer;

    void Start()
    {
        ProjectTrack();
    }

    void ProjectTrack()
    {
        lineRenderer = GetComponent<LineRenderer>();

        Vector3 point = spline.GetPoint(0f);
        int steps = mapPrecision * spline.CurveCount;

        lineRenderer.positionCount = steps + 1;

        for (int i = 0; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            lineRenderer.SetPosition(i, new Vector3(point.x, point.y, point.z));
        }

        lineRenderer.SetPosition(steps, lineRenderer.GetPosition(2));
    }
}
