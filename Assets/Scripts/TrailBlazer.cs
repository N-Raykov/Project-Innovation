using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlazer : SplineWalker
{
    [SerializeField] AI_Test plane;
    [SerializeField] GameObject navPointPrefab;
    [SerializeField] float navPointDensity = 1f;
    [SerializeField] float maxOffset = 1f;

    [SerializeField] GameObject pathContainer;
    private float timeSinceLastNavPoint = 0f;

    private Vector3 previousPosition;

    protected override void Update()
    {
        base.Update();

        float distanceMoved = Vector3.Distance(previousPosition, transform.position);
        float speed = distanceMoved / Time.deltaTime;

        navPointDensity = speed / 15;

        if (Time.time - timeSinceLastNavPoint >= 1 / navPointDensity)
        {
            SetNavPoint();
            timeSinceLastNavPoint = Time.time;
        }

        previousPosition = transform.position;
    }

    private void SetNavPoint()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-maxOffset, maxOffset), 0f, Random.Range(-maxOffset, maxOffset));

        GameObject newNavPoint = Instantiate(navPointPrefab, transform.position + randomOffset, Quaternion.identity, pathContainer.transform);
        plane.NavPoints.Add(newNavPoint.transform);
    }
}
