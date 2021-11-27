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
                bikeSlipDown.SlipStart();
            }
        }
    }
}
