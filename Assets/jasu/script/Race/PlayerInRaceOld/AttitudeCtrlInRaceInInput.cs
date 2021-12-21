using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeCtrlInRaceInInput : AttitudeCtrlInRace
{
    [SerializeField]
    float padInputRangeX = 0.15f;

    Vector3 padPos;

    [SerializeField]
    float padLength = 3.5f;

   [Header("デバッグ用")]

    [SerializeField]
    float inputX;

    private void Start()
    {
        padPos = TetraInput.sTetraPad.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = 0f;
        if (TetraInput.sTetraPad.GetNumOnPad() > 0)
        {
            List<GameObject> onPadObjList = TetraInput.sTetraPad.GetObjectsOnPad();

            List<float> xLengthList = new List<float>();

            foreach(GameObject onPad in onPadObjList)
            {
                xLengthList.Add(onPad.transform.position.x - padPos.x);
            }

            float total = 0;
            foreach(float xLength in xLengthList)
            {
                total += xLength;
            }

            inputX = total / xLengthList.Count / padLength;

            //inputX = TetraInput.sTetraPad.GetVector().x / TetraInput.sTetraPad.GetNumOnPad();
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
        if (groundSensor.GetSensorActive())
        {
            if (angleX > slipAngleMax || angleX < slipAngleMin)
            {
                if (TetraInput.sTetraLever.GetPoweredOn())
                {
                    Debug.Log("無敵中姿勢崩した");
                    bikeSlipDown.CallSlipStart("medium");
                }
                else
                {
                    Debug.Log("姿勢崩した");
                    bikeSlipDown.CallSlipStart("small");
                }
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
