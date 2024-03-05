using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectSplineProximityTracker : MonoBehaviour
{
    [SerializeField] BezierSpline spline;
    [SerializeField] GameObject objectToTrack;

    void Update()
    {
        transform.position = spline.PointOnTrack(objectToTrack.transform.position);
    }
}
