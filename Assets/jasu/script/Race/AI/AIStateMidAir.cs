using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMidAir : AIState
{
    //[SerializeField]
    //GameObject playerObj = null;

    //[SerializeField]
    //RaceManager raceManager;

    [SerializeField]
    AttitudeCtrlInRace attitudeCtrl = null;

    [SerializeField]
    GroundSensor groundSensor = null;

    float dirRot = 0f;

    //[Header("パラメータ")]

    //[SerializeField, Tooltip("姿勢制御の成否判定距離")]
    //float attitudeDistance = 10f;

    public override void StateStart()
    {
        dirRot = Random.Range(-1, 1);
    }

    public override void StateUpdate()
    {
        // 姿勢制御
        attitudeCtrl.dirRot = 0;
        //float distanceToPlayer = raceManager.GetPositionInRace(gameObject.GetInstanceID()) - raceManager.GetPositionInRace(playerObj.GetInstanceID());
        //if (distanceToPlayer < -attitudeDistance) // 負けてるなら成功させる
        //{
        //    attitudeCtrl.dirRot = 0;
        //}
        //else if (distanceToPlayer > attitudeDistance)
        //{
        //    attitudeCtrl.dirRot = dirRot;
        //}
        //else 
        //{
        //    attitudeCtrl.dirRot = 0;
        //}
    }

    public override bool CheckShiftCondition()
    {
        if(!groundSensor.GetSensorActive())
        {
            return true;
        }
        return false;
    }
}
