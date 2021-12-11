using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForPlayer : OnDirt
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    BikeSlipDown bikeSlipDown;

    [SerializeField]
    RainbowSprite rainbowSprite;

    private void Update()
    {
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            rainbowSprite.SetActive(true);
        }
        else
        {
            rainbowSprite.SetActive(false);
        }

        
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 3))
        {
            if (hitInfo.transform.gameObject.tag == "Dirt" && !TetraInput.sTetraLever.GetPoweredOn())
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
            other.GetComponent<DirtSplash>().parentInstanceID == transform.gameObject.GetInstanceID())
        {
            return;
        }

        if (other.gameObject.tag == "Slip" && !TetraInput.sTetraLever.GetPoweredOn())
        {
            if (!bikeSlipDown.isSliping)
            {
                Debug.Log("敵にぶつかった");
                bikeSlipDown.SlipStart("small");
            }
            return;
        }

        //if(other.gameObject.tag == "Dirt" && !TetraInput.sTetraLever.GetPoweredOn())
        //{
        //    // 泥だまりのとき減速中なら滑らない
        //    // -180 ~ 180 に補正
        //    float angleX = transform.localRotation.eulerAngles.x;
        //    if (angleX > 180)
        //    {
        //        angleX -= 360;
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
