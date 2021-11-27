using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttitudeCtrlInRace : MonoBehaviourPunCallbacks
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
    protected float rotUp = -80f;

    [SerializeField, Range(0f, 90f)]
    protected float rotDown = 60f;

    [SerializeField, Range(-90f, 0f)]
    protected float slipAngleUp = -75f;

    [SerializeField, Range(0f, 90f)]
    protected float slipAngleDown = 55f;

    public float dirRot { get; set; } = 0;

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
        if (angleX < rotUp)
        {
            vec.x = rotUp;
            transform.localEulerAngles = vec;
        }
        else if (angleX > rotDown)
        {
            vec.x = rotDown;
            transform.localEulerAngles = vec;
        }

        
        if (bikeSlipDown.isSliping) // スリップ時
        {
            vec.x = 0f;
            transform.localEulerAngles = vec;
        }
        else // 回転
        {
            if (dirRot < 0 && angleX > rotUp &&
                !colliderSensorFront.GetExistInCollider() && !colliderSensorBack.GetExistInCollider())
            {
                rb.AddTorque(Vector3.right * torqueForceMultiply * dirRot, ForceMode.Acceleration);
            }
               
            if (dirRot > 0 && angleX < rotDown && 
                    !colliderSensorFront.GetExistInCollider() && !colliderSensorBack.GetExistInCollider())
            {
                rb.AddTorque(Vector3.right * torqueForceMultiply * dirRot, ForceMode.Acceleration);
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
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // -180 ~ 180 に補正
        float angleX = transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }

        // 接地時
        if (colliderSensorFront.GetExistInCollider() || colliderSensorBack.GetExistInCollider())
        {
            if (angleX > slipAngleDown || angleX < slipAngleUp)
            {
                bikeSlipDown.SlipStart();
            }
        }
    }
}
