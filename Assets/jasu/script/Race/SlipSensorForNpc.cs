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
                DirtInRace dirt = null;

                // -180 ~ 180 に補正
                float angleX = transform.localRotation.eulerAngles.x;
                if (angleX > 180)
                {
                    angleX -= 360;
                }

                // 泥だまりのとき減速中なら滑らない
                dirt = other.transform.parent.GetComponent<DirtInRace>();
                if (angleX >= -10f || dirt == null)
                {
                    bikeSlipDown.SlipStart();
                }
            }
        }
    }
}
