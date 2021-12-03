using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttitudeCtrlInRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected Rigidbody rb = null;

    [SerializeField]
    protected ColliderSensor colliderSensor = null;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    [Header("パラメータ")]

    //[SerializeField]
    //protected float torqueForceMultiply = 5f;

    //[SerializeField]
    //protected float correctionTorqueMultiply = 1f;

    [SerializeField, Range(-90f, 0f)]
    protected float rotMin = -80f;

    public float GetRotMin { get { return rotMin; } }

    [SerializeField, Range(0f, 90f)]
    protected float rotMax = 60f;

    public float GetRotMax { get { return rotMax; } }

    [SerializeField, Range(-90f, 0f)]
    protected float rotMinOnGround = -30f;

    public float GetRotMinOnGround { get { return rotMinOnGround; } }

    [SerializeField, Range(0f, 90f)]
    protected float rotMaxOnGround = 30f;

    public float GetRotMaxOnGround { get { return rotMaxOnGround; } }

    [SerializeField, Range(-90f, 0f)]
    protected float slipAngleMin = -60f;

    [SerializeField, Range(0f, 90f)]
    protected float slipAngleMax = 55f;

    [SerializeField, Range(0f, 1f)]
    protected float rotRate = 0.25f;

    public float angleX { get; protected set; } = 0;

    public float dirRot { get; set; } = 0;

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected void OnUpdate()
    {
        // -180 ~ 180 に補正
        angleX = transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }
        
        Vector3 angle = transform.localEulerAngles;

        if (bikeSlipDown.isSliping) // スリップ時
        {
            angle.x = 0f;
            transform.localEulerAngles = angle;
        }
        else 
        {
            // 接地時　転倒判定
            SlipCheck();

            // 角度制限
            float min = rotMin;
            float max = rotMax;
            if (colliderSensor.GetExistInCollider())
            {
                min = rotMinOnGround;
                max = rotMaxOnGround;
            }

            if (angleX < min)
            {
                angle.x = min;
                transform.localEulerAngles = angle;
            }
            else if (angleX > max)
            {
                angle.x = max;
                transform.localEulerAngles = angle;
            }

            // 回転
            if (dirRot < 0 && angleX > min)
            {
                //rb.AddTorque(Vector3.right * torqueForceMultiply * dirRot, ForceMode.Acceleration);
                Vector3 rotVec = transform.localEulerAngles;
                rotVec.x = Mathf.Abs(dirRot) * min;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotVec), rotRate);
            }
            else if (dirRot > 0 && angleX < max)
            {
                //rb.AddTorque(Vector3.right * torqueForceMultiply * dirRot, ForceMode.Acceleration);
                Vector3 rotVec = transform.localEulerAngles;
                rotVec.x = Mathf.Abs(dirRot) * max;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotVec), rotRate);
            }
            else
            {
                Vector3 rotVec = transform.localEulerAngles;
                rotVec.x = 0;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotVec), rotRate);
            }

            //// 接地時
            //if (colliderSensorFront.GetExistInCollider() || colliderSensorBack.GetExistInCollider())
            //{
            //    // 補正
            //    if (angleX < 0f)
            //    {
            //        //rb.AddTorque(Vector3.right * correctionTorqueMultiply, ForceMode.Acceleration);
            //    }
            //    else if (angleX > 0f)
            //    {
            //        rb.AddTorque(-Vector3.right * correctionTorqueMultiply, ForceMode.Acceleration);
            //    }
            //}
        }
    }

    // 接地時　転倒判定
    virtual protected void SlipCheck()
    {
        if (colliderSensor.GetExistInCollider())
        {
            if (angleX > slipAngleMax || angleX < slipAngleMin)
            {
                bikeSlipDown.SlipStart();
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
    //        if (angleX > slipAngleMax || angleX < slipAngleMin)
    //        {
    //            bikeSlipDown.SlipStart();
    //        }
    //    }
    //}

    public float CorrectAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }

        return angle;
    }

    public void RestrictAngle()
    {
        // -180 ~ 180 に補正
        angleX = transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }

        // 角度制限
        Vector3 angle = transform.localEulerAngles;

        float min = rotMin;
        float max = rotMax;
        if (colliderSensor.GetExistInCollider())
        {
            min = rotMinOnGround;
            max = rotMaxOnGround;
        }

        if (angleX < min)
        {
            angle.x = min;
            transform.localEulerAngles = angle;
        }
        else if (angleX > max)
        {
            angle.x = max;
            transform.localEulerAngles = angle;
        }
    }
}
