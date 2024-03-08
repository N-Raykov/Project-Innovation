using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectSplineProximityTracker : MonoBehaviour
{
    [SerializeField] BezierSpline spline;
    [SerializeField] GameObject objectToTrack;
    [SerializeField] bool tracksPlayer;

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
        if (tracksPlayer == true && objectToTrack == null)
        {
            objectToTrack = GameObject.FindGameObjectWithTag("Player");
        }
        Vector3 pointOnTrack = spline.PointOnTrack(objectToTrack.transform.position);
        transform.position = new Vector3(pointOnTrack.x, -40, pointOnTrack.z);
    }
}
