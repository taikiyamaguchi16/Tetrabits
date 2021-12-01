using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSensorForPlayer : MonoBehaviour
{
    [SerializeField]
    BikeSlipDown bikeSlipDown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DirtSplash>() != null &&
            other.GetComponent<DirtSplash>().parentInstanceID == transform.gameObject.GetInstanceID())
        {
            return;
        }

        if (other.gameObject.tag == "Slip")
        {
            if (!bikeSlipDown.isSliping)
            {
                //bikeSlipDown.SlipStart("small");
            }
        }
    }
}
