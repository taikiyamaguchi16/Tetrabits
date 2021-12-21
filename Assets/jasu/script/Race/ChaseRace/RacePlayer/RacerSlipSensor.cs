using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerSlipSensor : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    BikeSlipDown bikeSlipDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (racerController.GetChaseRaceManager().started)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                bikeSlipDown.CallSlipStart("small");
            }

            if (bikeSlipDown.isSliping)
            {
                racerController.GetRacerMove().movable = false;
                racerController.GetRacerJump().jumpable = false;
                racerController.GetRacerLaneShift().movable = false;
            }
            else
            {
                racerController.GetRacerMove().movable = true;
                racerController.GetRacerJump().jumpable = true;
                racerController.GetRacerLaneShift().movable = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Slip" && !bikeSlipDown.isSliping)
        {
            bikeSlipDown.CallSlipStart("small");
        }
    }
}
