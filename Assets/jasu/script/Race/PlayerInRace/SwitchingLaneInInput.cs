using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingLaneInInput : MonoBehaviour
{
    [SerializeField]
    MoveBetweenLane moveBetweenLane;

    [SerializeField]
    float padInputRangeY = 0.35f;

    // Update is called once per frame
    void Update()
    {
        if(TetraInput.sTetraPad.GetVector().y < -padInputRangeY)
        {
            if(moveBetweenLane.belongingLaneId != 0)
            {
                moveBetweenLane.SetMoveLane(0);
            }
        }
        else if(TetraInput.sTetraPad.GetVector().y > padInputRangeY)
        {
            if (moveBetweenLane.belongingLaneId != 2)
            {
                moveBetweenLane.SetMoveLane(2);
            }
        }
        else
        {
            if (moveBetweenLane.belongingLaneId != 1)
            {
                moveBetweenLane.SetMoveLane(1);
            }
        }
    }
}
