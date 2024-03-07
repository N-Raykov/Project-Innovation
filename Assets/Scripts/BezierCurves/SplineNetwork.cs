using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SplineNetwork : MonoBehaviour
{
    [SerializeField] private List<SplitPoint> splitPoints = new List<SplitPoint>();
    [SerializeField] public List<Connection> connections = new List<Connection>();

    private static SplineNetwork instance;
    public static SplineNetwork Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SplineNetwork>();
            }
            return instance;
        }
    }

    public void AddConnection(BezierSpline main, BezierSpline branch)
    {
        if (!connections.Any(c => c.main == main && c.connection == branch))
        {
            connections.Add(new Connection(main, branch));
        }
        else
        {
            Debug.LogWarning("Connection already exists.");
        }
    }

    public void DisconnectSplines(BezierSpline main, BezierSpline branch)
    {
        Connection connectionToRemove = connections.FirstOrDefault(c => c.main == main && c.connection == branch);
        if (connectionToRemove != null)
        {
            connections.Remove(connectionToRemove);
        }
        else
        {
            Debug.LogWarning("Connection doesn't exist");
        }
    }

    public bool TryGetConnectedSpline(BezierSpline spline, out BezierSpline connectedSpline)
    {
        foreach (var connection in connections)
        {
            if (connection.main == spline)
            {
                connectedSpline = connection.connection;
                return true;
            }
        }

        connectedSpline = null;
        return false;
    }

    public bool TryGetBranchSpline(int controlPointIndex, out BezierSpline branchSpline, out BezierSpline mainSpline)
    {
        foreach (var splitPoint in splitPoints)
        {
            if (splitPoint.controlPointIndex == controlPointIndex)
            {
                branchSpline = splitPoint.spline;
                mainSpline = splitPoint.main;
                return true;
            }
        }
        mainSpline = null;
        branchSpline = null;
        return false;
    }

    public bool AddSplitPoint(int controlPointIndex, BezierSpline spline, BezierSpline main)
    {
        if (!splitPoints.Exists(point => point.controlPointIndex == controlPointIndex))
        {
            splitPoints.Add(new SplitPoint(controlPointIndex, spline, main));
            return true; 
        }
        else
        {
            DestroyImmediate(spline.gameObject);
            Debug.LogWarning("Duplicate split point found: " + controlPointIndex);
            return false; 
        }
    }

    public bool RemoveSplitPoint(int controlPointIndex)
    {
        int indexToRemove = splitPoints.FindIndex(point => point.controlPointIndex == controlPointIndex);
        if (indexToRemove != -1)
        {
            BezierSpline connectedSpline;
            SplitPoint splitPoint = splitPoints[indexToRemove];
            if (TryGetConnectedSpline(splitPoints[indexToRemove].spline, out connectedSpline))
            {
                connections.RemoveAt(indexToRemove);
            }
            splitPoints.RemoveAt(indexToRemove);
            DestroyImmediate(splitPoint.spline.gameObject);
            return true;
        }
        else
        {
            Debug.LogWarning("Split point index not found: " + controlPointIndex);
            return false;
        }
    }

}
