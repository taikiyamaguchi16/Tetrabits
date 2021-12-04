using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeCtrlInRaceInInput : AttitudeCtrlInRace
{
    [SerializeField]
    float padInputRangeX = 0.15f;

    // Update is called once per frame
    void Update()
    {
        dirRot = 0f;
        if(TetraInput.sTetraPad.GetVector().x < -padInputRangeX ||
            TetraInput.sTetraPad.GetVector().x > padInputRangeX)
        {
            dirRot = TetraInput.sTetraPad.GetVector().x;
        }

        OnUpdate();
    }

    protected override void SlipCheck()
    {
        if (colliderSensor.GetExistInCollider())
        {
            if (angleX > slipAngleMax || angleX < slipAngleMin)
            {
                bikeSlipDown.SlipStart("small");
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // -180 ~ 180 に補正
    //    float angleX = transform.localRotation.eulerAngles.x;
    //    if (angleX > 180)
    //    {
    //        angleX -= 360;
    //    }

    //    // 接地時
    //    if (colliderSensorFront.GetExistInCollider() || colliderSensorBack.GetExistInCollider())
    //    {
    //        if (angleX > slipAngleDown || angleX < slipAngleUp)
    //        {
    //            bikeSlipDown.SlipStart("small");
    //        }
    //    }
    //}
}
