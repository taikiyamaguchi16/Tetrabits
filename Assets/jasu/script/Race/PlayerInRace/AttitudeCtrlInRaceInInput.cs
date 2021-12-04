using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeCtrlInRaceInInput : AttitudeCtrlInRace
{
    [SerializeField]
    float padInputRangeX = 0.15f;

    [Header("デバッグ用")]

    [SerializeField]
    float inputX;

    // Update is called once per frame
    void Update()
    {
        inputX = 0f;
        if (TetraInput.sTetraPad.GetNumOnPad() > 0)
        {
            inputX = TetraInput.sTetraPad.GetVector().x / TetraInput.sTetraPad.GetNumOnPad();
        }

        dirRot = 0f;

        if (inputX < -padInputRangeX ||
            inputX > padInputRangeX)
        {
            dirRot = inputX;
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
