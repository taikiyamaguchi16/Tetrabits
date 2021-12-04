using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForNpc : MonoBehaviour
{
    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

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
                // -180 ~ 180 に補正
                float angleX = transform.localRotation.eulerAngles.x;
                if (angleX > 180)
                {
                    angleX -= 360;
                }

                // 泥だまりのとき減速中なら滑らない
                if (angleX >= -10f || other.transform.parent.tag != "Dirt")
                {
                    bikeSlipDown.SlipStart();
                }
            }
        }
    }
}
