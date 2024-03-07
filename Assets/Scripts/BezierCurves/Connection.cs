using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connection
{
    public BezierSpline main;
    public BezierSpline connection;

    public Connection(BezierSpline pMain, BezierSpline pConnection)
    {
        this.main = pMain;
        this.connection = pConnection;
    }
}
