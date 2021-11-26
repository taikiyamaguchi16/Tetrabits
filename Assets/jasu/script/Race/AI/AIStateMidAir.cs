using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMidAir : AIState
{
    [SerializeField]
    GameObject phisicalParts;

    [SerializeField]
    AttitudeCtrlInRace attitudeCtrlInRace = null;

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    [SerializeField,Range(0, 2)]
    float min = 0f;

    [SerializeField,Range(0, 2)]
    float max = 10f;

    float dirRot = 1f;

    public override void StateStart()
    {
        dirRot = Random.Range(min, max);
    }

    public override void StateUpdate()
    {
        // -180 ~ 180 に補正
        float angleX = phisicalParts.transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }

        if(angleX > 5)
        {
            attitudeCtrlInRace.dirRot = -dirRot;
        }
        else if(angleX < -5)
        {
            attitudeCtrlInRace.dirRot = dirRot;
        }
    }

    public override bool CheckShiftCondition()
    {
        if(!colliderSensorFront.GetExistInCollider() &&
            !colliderSensorBack.GetExistInCollider())
        {
            return true;
        }
        return false;
    }
}
