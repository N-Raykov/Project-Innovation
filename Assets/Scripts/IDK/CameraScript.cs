using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    GameObject Target;

    [SerializeField]
    Vector3 Offset;

    Rigidbody targetBody;

    // Start is called before the first frame update
    void Start()
    {
        targetBody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Target.transform.position +
        (Target.transform.forward * Offset.z) +
        (Target.transform.up * Offset.y);

        transform.position = Vector3.Slerp(transform.position, targetPos, 5.0f * Time.deltaTime);
        transform.LookAt(targetPos);
    }
}
