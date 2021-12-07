using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateOnGround : AIState
{
    [SerializeField]
    GameObject playerObj = null;

    [SerializeField]
    GameObject[] otherRacers; 

    //[SerializeField]
    //AISensorDistanceTarget sensorDistanceTarget;

    [SerializeField]
    DirtSensor dirtSensor;

    [SerializeField]
    MoveBetweenLane moveBetweenLane;

    [SerializeField]
    AttitudeCtrlInRace attitudeCtrl;

    //[SerializeField]
    //DirtSplashSpawn dirtSplashSpawn;

    [SerializeField]
    ColliderSensor colliderSensor = null;

    //[SerializeField]
    //RaceManager raceManager = null;

    [Header("パラメータ")]

    //[SerializeField, Tooltip("加速距離")]
    //float accelerateDistance = 10f;

    //[SerializeField, Tooltip("減速距離")]
    //float decelerateDistance = 10f;

    //[SerializeField]
    //float correctSeconds = 1f;

    //float correctTimer = 0f;

    //[SerializeField]
    //float dirtIntervalSeconds = 3f;

    //[SerializeField]
    //float distanceToPlayer;

    List<float> distances = new List<float>();

    public override void StateStart()
    {

    }

    public override void StateUpdate()
    {
        // 姿勢制御
        attitudeCtrl.dirRot = 1f; // 基本加速
        //distanceToPlayer = raceManager.GetPositionInRace(gameObject.GetInstanceID()) - raceManager.GetPositionInRace(playerObj.GetInstanceID());
        //if (distanceToPlayer > decelerateDistance)
        //{
        //    attitudeCtrl.dirRot = -0.8f;
        //}
        //else 
        //if (distanceToPlayer < -accelerateDistance)
        //{
        //    attitudeCtrl.dirRot = 1f;
        //}

        if (dirtSensor.sensorActive)    // 泥あったら減速
        {
            attitudeCtrl.dirRot = -1f;
        }

        // レーン移動
        distances.Add(Vector3.Distance(transform.position, playerObj.transform.position));
        foreach (var other in otherRacers)
        {
            distances.Add(Vector3.Distance(transform.position, other.transform.position));
        }

        //for(int i = 0; i< )

        //if (sensorDistanceTarget.sensorActive && moveBetweenLane.belongingLaneId != playerMoveBetweenLane.belongingLaneId && canBeDirt)
        //{
        //    correctTimer += Time.deltaTime;
        //    if (correctTimer > correctSeconds)
        //    {
        //        correctTimer = 0f;
        //        moveBetweenLane.SetMoveLane(playerMoveBetweenLane.belongingLaneId);
        //    }
        //}
        //else
        //{
        //    correctTimer += Time.deltaTime;
        //    if (correctTimer > correctSeconds)
        //    {
        //        correctTimer = 0f;
        //        moveBetweenLane.SetMoveLane(moveBetweenLane.belongingLaneId + Random.Range(-1, 1));
        //    }
        //}

        // 泥掛け
        //dirtIntervalTimer += Time.deltaTime;
        //if(dirtIntervalTimer > dirtIntervalSeconds)
        //{
        //    canBeDirt = true;
        //}

        //if (moveBetweenLane.belongingLaneId == playerMoveBetweenLane.belongingLaneId && sensorDistanceTarget.sensorActive && canBeDirt)
        //{
        //    canBeDirt = false;
        //    dirtIntervalTimer = 0f;

        //    if(moveBetweenLane.belongingLaneId - 1 < 0)
        //    {
        //        moveBetweenLane.SetMoveLane(1);
        //    }
        //    else if(moveBetweenLane.belongingLaneId + 1 > moveBetweenLane.GetLaneNum())
        //    {
        //        moveBetweenLane.SetMoveLane(moveBetweenLane.GetLaneNum() - 1);
        //    }
        //    else
        //    {
        //        moveBetweenLane.SetMoveLane(moveBetweenLane.belongingLaneId + 1);
        //    }
            

        //    dirtSplashSpawn.dirtSplashFlag = true;
        //}
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
