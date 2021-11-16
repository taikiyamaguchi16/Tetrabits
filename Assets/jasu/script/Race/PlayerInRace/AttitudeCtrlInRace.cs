using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeCtrlInRace : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb = null;

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    [SerializeField]
    BikeSlipDown bikeSlipDown = null;

    [SerializeField]
    float padInputRangeX = 0.3f;

    [SerializeField]
    float torqueForceMultiply = 5f;

    [SerializeField]
    float correctionTorqueMultiply = 1f;

    [SerializeField, Range(-90f, 0f)]
    float rotMin = -60f;

    [SerializeField, Range(0f, 90f)]
    float rotMax = 60f;

    [SerializeField, Range(0f, 90f)]
    float slipAngle = 55f;

    // Update is called once per frame
    void Update()
    {
        // -180 ~ 180 に補正
        float angleX = transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }

        // 角度制限
        Vector3 vec = transform.localEulerAngles;
        if (angleX < rotMin)
        {
            vec.x = rotMin;
            transform.localEulerAngles = vec;
        }
        else if (angleX > rotMax)
        {
            vec.x = rotMax;
            transform.localEulerAngles = vec;
        }

        
        if (bikeSlipDown.isSliping) // スリップ時
        {
            vec.x = 0f;
            transform.localEulerAngles = vec;
        }
        else // 回転
        {
            // 入力による回転
            float padVecX = TetraInput.sTetraPad.GetVector().x;
            if (padVecX < -padInputRangeX && angleX > rotMin)
            {
                rb.AddTorque(Vector3.right * padVecX * torqueForceMultiply, ForceMode.Acceleration);
            }
               
            if (padVecX > padInputRangeX && angleX < rotMax && 
                    !colliderSensorFront.GetExistInCollider() && !colliderSensorBack.GetExistInCollider())
            {
                rb.AddTorque(Vector3.right * padVecX * torqueForceMultiply, ForceMode.Acceleration);
            }

            // 接地時
            if (colliderSensorFront.GetExistInCollider() || colliderSensorBack.GetExistInCollider())
            {
                // 補正
                if (angleX < 0f)
                {
                    //rb.AddTorque(Vector3.right * correctionTorqueMultiply, ForceMode.Acceleration);
                }
                else if (angleX > 0f)
                {
                    rb.AddTorque(-Vector3.right * correctionTorqueMultiply, ForceMode.Acceleration);
                }

                if(angleX > slipAngle)
                {
                    bikeSlipDown.SlipStart();
                }
            }
        }
    }
}
