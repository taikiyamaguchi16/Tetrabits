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
    BikeSensor frontBikeSensor;

    [SerializeField]
    BikeSensor widthBikeSensor;

    [SerializeField]
    BikeSensor nearBikeSensor;

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
        // スロープ上にいるか
        bool onSlope = false;
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, new Vector3(0, -1, 1));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 3))
        {
            if (hitInfo.transform.gameObject.tag == "SlopeRoadInRace" ||
            hitInfo.transform.parent.gameObject.tag == "SlopeRoadInRace")
            {
                onSlope = true;
            }
        }

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
            //attitudeCtrl.dirRot = -1f;
            ShiftLane();
        }

        if (onSlope)   // 坂道では中速
        {
            attitudeCtrl.dirRot = 0f;
        }

        // レーン移動
        if (widthBikeSensor.sensorActive)  // 横のレーンに他のバイクがいるか
        {
            if (nearBikeSensor.sensorActive)
            {
                ShiftLane();
            }
        }
        else
        {
            if (frontBikeSensor.sensorActive)
            {
                ShiftLane();
            }
        }

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

    private void ShiftLane()
    {
        int moveLaneId = moveBetweenLane.belongingLaneId;
        if (moveBetweenLane.belongingLaneId <= 0)
        {
            moveLaneId = 1;
        }
        else if (moveBetweenLane.belongingLaneId >= moveBetweenLane.GetLaneNum() - 1)
        {
            moveLaneId = moveBetweenLane.GetLaneNum() - 2;
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                moveLaneId--;
            }
            else
            {
                moveLaneId++;
            }
        }
        moveBetweenLane.SetMoveLane(moveLaneId);
    }
}
