using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectSplineProximityTracker : MonoBehaviour
{
    [SerializeField] BezierSpline spline;
    [SerializeField] GameObject objectToTrack;

    private void Start()
    {
        if (spline == null)
        {
            Debug.LogWarning("Tracker is not assigned to a spline");
            spline = null;
        }
    }

    void Update()
    {
        Vector3 pointOnTrack = spline.PointOnTrack(objectToTrack.transform.position);
        transform.position = new Vector3(pointOnTrack.x, 4, pointOnTrack.z);
    }
}
