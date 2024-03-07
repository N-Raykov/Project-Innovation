using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplitPoint
{
    public int controlPointIndex;
    public BezierSpline spline;
    public BezierSpline main;
    public SplitPoint(int index, BezierSpline spline, BezierSpline main)
    {
        controlPointIndex = index;
        this.spline = spline;
        this.main = main;
    }
}

