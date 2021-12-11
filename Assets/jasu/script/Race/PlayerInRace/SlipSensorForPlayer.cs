using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForPlayer : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    BikeSlipDown bikeSlipDown;

    [SerializeField]
    RainbowSprite rainbowSprite;

    bool slowDownFlag = false;

    [SerializeField, Range(0f, 1f)]
    float slowMultipy = 0.5f;

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

        //if (slowDownFlag)
        //{

        //}
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
                bikeSlipDown.SlipStart("small");
            }
            return;
        }

        if(other.gameObject.tag == "Dirt" && !TetraInput.sTetraLever.GetPoweredOn())
        {
            // 泥だまりのとき減速中なら滑らない
            // -180 ~ 180 に補正
            float angleX = transform.localRotation.eulerAngles.x;
            if (angleX > 180)
            {
                angleX -= 360;
            }

            if (angleX >= -10f)
            {
                Vector3 velocity = rb.velocity;
                velocity.z = 0;
                rb.velocity = velocity;
                slowDownFlag = true;
            }
        }
    }
}
