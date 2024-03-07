using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speedMod = 10.0f;
    [SerializeField] Vector3 offset;
    private Vector3 point;

    void Start()
    {
        point = target.transform.position + offset;
        transform.LookAt(point);
    }

    void Update()
    {
        transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
    }
}
