using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotater : MonoBehaviour
{
    [SerializeField] float rotateSpeedMax = 30.0f;
    [SerializeField] float rotateAcceleration = 10.0f;
    float rotateSpeed;

    bool rotating;

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            rotateSpeed += rotateAcceleration * Time.deltaTime;
            if (rotateSpeed >= rotateSpeedMax)
            {
                rotateSpeed = rotateSpeedMax;
            }
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotateSpeed, Vector3.right);
        }
    }

    public void StartUpRotating()
    {
        rotating = true;
    }

}
