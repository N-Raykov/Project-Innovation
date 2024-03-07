using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroTesting : MonoBehaviour
{
    void GyroModifyCamera()
    {
        transform.rotation = GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        print(q);
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    // Update is called once per frame
    void Update()
    {
        GyroModifyCamera();  
    }
}
