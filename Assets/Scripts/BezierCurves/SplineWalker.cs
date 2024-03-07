using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    [SerializeField] private BezierSpline spline;
    [SerializeField] private float speed;
    [SerializeField] private bool lookForward;
    [SerializeField] private SplineWalkerMode mode;
    [SerializeField] float chanceToSwitchSpline = 0.5f;

    private float progress;
    private bool goingForward = true;
    private float splineLength;
    private SplineNetwork splineNetwork;

    private int lastControlPointIndex = -1;

    private void Start()
    {
        splineLength = spline.GetLength();
        splineNetwork = spline.GetComponentInParent<SplineNetwork>();
    }

    private void Update()
    {
        float step = speed / splineLength;

        if (goingForward)
        {
            progress += step * Time.deltaTime;
            if (progress > 1f)
            {
                BezierSpline connectedSpline;
                if (SplineNetwork.Instance.TryGetConnectedSpline(spline, out connectedSpline))
                {
                    spline = connectedSpline;
                    splineLength = spline.GetLength();

                    Vector3 nearestPoint = spline.PointOnTrack(transform.position);

                    progress = spline.GetTime(nearestPoint);

                    goingForward = true;
                }
                else
                {
                    if (mode == SplineWalkerMode.Once)
                    {
                        progress = 1f;
                    }
                    else if (mode == SplineWalkerMode.Loop)
                    {
                        progress -= 1f;
                    }
                    else
                    {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            }
        }
        else
        {
            progress -= step * Time.deltaTime;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }

        Vector3 targetPosition = spline.GetPoint(progress);
        Quaternion targetRotation = Quaternion.LookRotation(spline.GetDirection(progress), Vector3.up);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        if (lookForward)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        int currentControlPointIndex = Mathf.RoundToInt(progress * (spline.ControlPointCount - 1));

        if (currentControlPointIndex != lastControlPointIndex)
        {
            CheckForBranchSpline(currentControlPointIndex);
            lastControlPointIndex = currentControlPointIndex;
        }
    }

    private void CheckForBranchSpline(int currentControlPointIndex)
    {
        if (splineNetwork == null)
            return;

        BezierSpline branchSpline;
        BezierSpline mainSpline;
        if (splineNetwork.TryGetBranchSpline(currentControlPointIndex, out branchSpline, out mainSpline))
        {
            if (Random.value <= chanceToSwitchSpline && spline == mainSpline)
            {
                spline = branchSpline;
                splineLength = spline.GetLength();
                progress = 0f;
                goingForward = true;
            }
        }
    }
}


