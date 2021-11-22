using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeCtrlInRace : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rb = null;

    [SerializeField]
    protected ColliderSensor colliderSensorFront = null;

    [SerializeField]
    protected ColliderSensor colliderSensorBack = null;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    [SerializeField]
    protected float torqueForceMultiply = 5f;

    [SerializeField]
    protected float correctionTorqueMultiply = 1f;

    [SerializeField, Range(-90f, 0f)]
    protected float rotMin = -60f;

    [SerializeField, Range(0f, 90f)]
    protected float rotMax = 60f;

    [SerializeField, Range(0f, 90f)]
    protected float slipAngle = 55f;

    public int dirRot { get; set; } = 0;

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
            if (dirRot < 0 && angleX > rotMin)
            {
                rb.AddTorque(Vector3.right * torqueForceMultiply, ForceMode.Acceleration);
            }
               
            if (dirRot > 0 && angleX < rotMax && 
                    !colliderSensorFront.GetExistInCollider() && !colliderSensorBack.GetExistInCollider())
            {
                rb.AddTorque(Vector3.right * torqueForceMultiply, ForceMode.Acceleration);
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
