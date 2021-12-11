﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForNpc : OnDirt
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    private void Update()
    {
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 3))
        {
            if (hitInfo.transform.gameObject.tag == "Dirt")
            {
                float angleX = transform.localRotation.eulerAngles.x;
                if (angleX > 180)
                {
                    angleX -= 360;
                }

                if (angleX >= -10f && !slowDownFlag)
                {
                    Vector3 velocity = rb.velocity;
                    velocity.z = 0;
                    rb.velocity = velocity;
                    slowDownFlag = true;
                }
            }
            else
            {
                slowDownFlag = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DirtSplash>() != null &&
            other.GetComponent<DirtSplash>().parentInstanceID == gameObject.GetInstanceID())
        {
            return;
        }

        if (other.gameObject.tag == "Slip")
        {
            if (!bikeSlipDown.isSliping)
            {
                bikeSlipDown.SlipStart("small");
            }
            return;
        }

        //if (other.gameObject.tag == "Dirt")
        //{
        //    // 泥だまりのとき減速中なら滑らない
        //    // -180 ~ 180 に補正
        //    float angleX = transform.localRotation.eulerAngles.x;
        //    if (angleX > 180)
        //    {
        //        angleX -= 360;
        //    }

        //    if (angleX >= -10f)
        //    {
        //        Vector3 velocity = rb.velocity;
        //        velocity.z = 0;
        //        rb.velocity = velocity;
        //        slowDownFlag = true;
        //    }
        //}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Dirt")
    //    {
    //        slowDownFlag = false;
    //    }
    //}
}
