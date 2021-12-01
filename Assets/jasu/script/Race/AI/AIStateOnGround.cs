using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateOnGround : AIState
{
    [SerializeField]
    AISensorDistanceTarget sensorDistanceTarget;

    [SerializeField]
    MoveBetweenLane moveBetweenLane;

    [SerializeField]
    MoveBetweenLane playerMoveBetweenLane;

    [SerializeField]
    DirtSplashSpawn dirtSplashSpawn;

    [SerializeField]
    ColliderSensor colliderSensor = null;

    [SerializeField]
    float correctSeconds = 1f;

    float correctTimer = 0f;

    [SerializeField]
    float dirtIntervalSeconds = 5f;

    float dirtIntervalTimer = 0f;

    bool canBeDirt = true;

    public override void StateStart()
    {

    }

    public override void StateUpdate()
    {
        if (sensorDistanceTarget.sensorActive && moveBetweenLane.belongingLaneId != playerMoveBetweenLane.belongingLaneId)
        {
            correctTimer += Time.deltaTime;
            if(correctTimer > correctSeconds)
            {
                correctTimer = 0f;
                moveBetweenLane.SetMoveLane(playerMoveBetweenLane.belongingLaneId);
            }
        }

        // 泥掛け
        dirtIntervalTimer += Time.deltaTime;
        if(dirtIntervalTimer > dirtIntervalSeconds)
        {
            canBeDirt = true;
        }

        if (moveBetweenLane.belongingLaneId == playerMoveBetweenLane.belongingLaneId && sensorDistanceTarget.sensorActive && canBeDirt)
        {
            canBeDirt = false;
            dirtIntervalTimer = 0f;

            dirtSplashSpawn.dirtSplashFlag = true;
        }
    }

    public override bool CheckShiftCondition()
    {
        if (colliderSensor.GetExistInCollider())
        {
            return true;
        }
        return false;
    }
}
