﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateOnGround : AIState
{
    [SerializeField]
    GameObject playerObj = null;

    [SerializeField]
    GameObject[] otherNPCs; 

    [SerializeField]
    AISensorDistanceTarget sensorDistanceTarget;

    [SerializeField]
    MoveBetweenLane moveBetweenLane;

    [SerializeField]
    MoveBetweenLane playerMoveBetweenLane;

    [SerializeField]
    MoveBetweenLane[] otherMoveBetweenLane;

    [SerializeField]
    AttitudeCtrlInRace attitudeCtrl;

    [SerializeField]
    DirtSplashSpawn dirtSplashSpawn;

    [SerializeField]
    ColliderSensor colliderSensor = null;

    [SerializeField]
    RaceManager raceManager = null;

    [Header("パラメータ")]

    [SerializeField, Tooltip("加速距離")]
    float accelerateDistance = 10f;

    [SerializeField, Tooltip("減速距離")]
    float decelerateDistance = 10f;

    [SerializeField]
    float correctSeconds = 1f;

    float correctTimer = 0f;

    [SerializeField]
    float dirtIntervalSeconds = 5f;

    float dirtIntervalTimer = 0f;

    bool canBeDirt = true;

    [SerializeField]
    float distanceToPlayer;

    List<float> distances = new List<float>();

    public override void StateStart()
    {

    }

    public override void StateUpdate()
    {
        // 姿勢制御
        attitudeCtrl.dirRot = 0f;
        distanceToPlayer = raceManager.GetPositionInRace(gameObject.GetInstanceID()) - raceManager.GetPositionInRace(playerObj.GetInstanceID());
        //if (distanceToPlayer > decelerateDistance)
        //{
        //    attitudeCtrl.dirRot = -0.8f;
        //}
        //else 
        if (distanceToPlayer < -accelerateDistance)
        {
            attitudeCtrl.dirRot = 0.8f;
        }

        // レーン移動
        distances.Add(Vector3.Distance(transform.position, playerObj.transform.position));
        foreach (var other in otherNPCs)
        {
            distances.Add(Vector3.Distance(transform.position, other.transform.position));
        }

        //for(int i = 0; i< )

        if (sensorDistanceTarget.sensorActive && moveBetweenLane.belongingLaneId != playerMoveBetweenLane.belongingLaneId)
        {
            correctTimer += Time.deltaTime;
            if (correctTimer > correctSeconds)
            {
                correctTimer = 0f;
                moveBetweenLane.SetMoveLane(playerMoveBetweenLane.belongingLaneId);
            }
        }
        else
        {
            correctTimer += Time.deltaTime;
            if (correctTimer > correctSeconds)
            {
                correctTimer = 0f;
                moveBetweenLane.SetMoveLane(moveBetweenLane.belongingLaneId + Random.Range(-1, 1));
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
