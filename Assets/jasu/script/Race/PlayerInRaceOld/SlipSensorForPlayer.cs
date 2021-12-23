using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForPlayer : OnDirt
{

    [SerializeField]
    float dirtSlipOnLeverSeconds = 3f;


    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    BikeSlipDown bikeSlipDown;

    [SerializeField]
    RainbowSprite rainbowSprite;

    [SerializeField]
    AccelerateInInput accelerateInInput;

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
            if (hitInfo.transform.gameObject.tag == "Dirt")
            {
                float angleX = transform.localRotation.eulerAngles.x;
                if (angleX > 180)
                {
                    angleX -= 360;
                }

                if (angleX >= -10f && !onDirt)
                {
                    if (TetraInput.sTetraLever.GetPoweredOn())
                    {
                        Vector3 velocity = rb.velocity;
                        velocity.z /= 2;
                        rb.velocity = velocity;
                        dirtSlipTimer = 0f;
                        onDirt = true;
                    }
                    else
                    {
                        Vector3 velocity = rb.velocity;
                        velocity.z = 0;
                        rb.velocity = velocity;
                        dirtSlipTimer = 0f;
                        onDirt = true;
                    }
                }
            }
            else
            {
                onDirt = false;
            }
        }

        if (onDirt)
        {
            dirtSlipTimer += Time.deltaTime;
            if (TetraInput.sTetraLever.GetPoweredOn())
            {
                if (dirtSlipTimer > dirtSlipOnLeverSeconds)
                {
                    dirtSlipTimer = 0f;
                    bikeSlipDown.CallSlipStart("small");
                }
            }
            else
            {
                if (dirtSlipTimer > dirtSlipSeconds)
                {
                    dirtSlipTimer = 0f;
                    bikeSlipDown.CallSlipStart("medium");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<DirtSplash>() != null &&
        //    other.GetComponent<DirtSplash>().parentInstanceID == transform.gameObject.GetInstanceID())
        //{
        //    return;
        //}

        if (other.gameObject.tag == "Slip" && !accelerateInInput.bodyBlowActive)
        {
            if (!bikeSlipDown.isSliping)
            {
                //Debug.Log("敵にぶつかった");
                bikeSlipDown.CallSlipStart("small");
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
    //        onDirt = false;
    //    }
    //}
}
