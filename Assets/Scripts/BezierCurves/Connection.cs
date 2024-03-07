using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connection
{
    public BezierSpline main;
    public BezierSpline branch;

    public Connection(BezierSpline pMain, BezierSpline pBranch)
    {
        this.main = pMain;
        this.branch = pBranch;
    }
}
