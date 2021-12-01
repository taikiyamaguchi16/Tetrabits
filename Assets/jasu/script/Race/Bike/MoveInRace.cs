using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class MoveInRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public Rigidbody rb { get; protected set; }

    [SerializeField]
    protected ColliderSensor colliderSensor = null;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    [SerializeField]
    protected DirtSplashSpawn dirtSplashSpawn;

    [SerializeField]
    protected AttitudeCtrlInRace attitudeCtrl;

    [Header("パラメータ")]
    
    [SerializeField]
    protected float moveSpd;

    [SerializeField,Tooltip("移動速度")]
    protected float moveSpdStandard = 10f;

    [SerializeField, Tooltip("加速倍率")]
    protected float accelerateMultiply = 1.3f;

    [SerializeField, Tooltip("減速倍率")]
    protected float decelerateMultiply = 0.7f;

    [SerializeField, Tooltip("坂を上る時移動速度にかける倍率")]
    protected float moveSlopeMultiply = 2f;

    [SerializeField,Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    protected float moveForceMultiplyStart = 5f;

    [SerializeField, Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    protected float moveForceMultiplyStop = 5f;

    [SerializeField, Tooltip("坂登る時の回転率")]
    protected float rotLateSlope = 0.25f;

    [SerializeField, Tooltip("平らな所にいる時の平面に戻る回転率")]
    protected float rotLateFlat = 0.25f;

    [SerializeField]
    protected float gravity = -100f; // 重力

    protected Vector3 groundNormalVec = Vector3.zero; // 地面の法線ベクトル

    [SerializeField]
    protected Vector3 moveVec = Vector3.zero;

    protected int wheelonSlopeNum = 0; 

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void LateUpdate()
    {
        // 移動速度決定
        SetMoveSpd();
    }

    private void FixedUpdate()
    {
        moveVec = Vector3.zero;

        // 移動ベクトル作成
        SetMoveVec();

        // 空中でのy軸の加速を切る
        SetGravity();

        // 加速
        AccelerationToMoveVec();
    }

    protected void SetMoveSpd()
    {
        // -180 ~ 180 に補正
        float angleX = transform.localRotation.eulerAngles.x;
        if (angleX > 180)
        {
            angleX -= 360;
        }

        moveSpd = moveSpdStandard;
        if (wheelonSlopeNum <= 0 && colliderSensor.GetExistInCollider())
        {
            if (angleX < 0)
            {
                float decelerate = 1 - ((1 - decelerateMultiply) * Mathf.Abs(angleX) / Mathf.Abs(attitudeCtrl.GetRotMinOnGround));
                moveSpd *= decelerate;
            }
            else if (angleX > 0)
            {
                float accelerate = 1 + ((accelerateMultiply - 1) * Mathf.Abs(angleX) / Mathf.Abs(attitudeCtrl.GetRotMinOnGround));
                moveSpd *= accelerate;
            }
        }
    }

    protected void SetMoveVec()
    {
        if (!bikeSlipDown.isSliping)
        {
            if (wheelonSlopeNum > 0)
            {
                moveVec = Vector3.ProjectOnPlane(Vector3.forward, groundNormalVec);

                transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.LookRotation(moveVec), rotLateSlope);
                if (attitudeCtrl.CorrectAngle(transform.parent.localRotation.eulerAngles.x) <= attitudeCtrl.CorrectAngle(Quaternion.LookRotation(moveVec).eulerAngles.x))
                {
                    transform.parent.localRotation = Quaternion.LookRotation(moveVec);
                }

                moveVec.y *= (moveSpd / moveVec.z) * moveSlopeMultiply;
            }
            else
            {
                groundNormalVec = Vector3.zero;

                // 空中でのみ重力
                if (colliderSensor.GetExistInCollider())
                {
                    transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.LookRotation(Vector3.forward), rotLateFlat);
                }
            }

            moveVec.z = moveSpd;
        }
    }

    // MoveVecのYに重力を掛ける
    protected void SetGravity()
    {
        // 空中でのみ重力
        if (!colliderSensor.GetExistInCollider())
        {
            wheelonSlopeNum = 0;
            moveVec.y = gravity;
        }
    }

    // FixedUpdateの最期で呼び出す　決定したMoveVecをもとにAddForceする
    protected void AccelerationToMoveVec()
    {
        if (moveVec.z <= 0f) // stop
        {
            rb.AddForce(moveForceMultiplyStop * (moveVec - rb.velocity), ForceMode.Acceleration);
        }
        else // start
        {
            rb.AddForce(moveForceMultiplyStart * (moveVec - rb.velocity), ForceMode.Acceleration);
        }

        dirtSplashSpawn.moveVec = moveVec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        WhenOnCollisionEnter(collision);
    }

    protected void WhenOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SlopeRoadInRace" ||
            collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
        {
            wheelonSlopeNum++;
            groundNormalVec = collision.contacts[0].normal;
            //Debug.Log("坂の法線取得" + groundNormalVec);
        }
        //else if ((collision.gameObject.tag == "FlatRoadInRace" ||
        //        collision.transform.parent.gameObject.tag == "FlatRoadInRace") && wheelonSlopeNum <= 0)
        //{
        //    groundNormalVec = Vector3.zero;
        //    //Debug.Log("坂の法線リセット" + groundNormalVec);
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        WhenOnCollisionExit(collision);
    }

    protected void WhenOnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "SlopeRoadInRace" ||
            collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
        {
            //wheelonSlopeNum--;
        }
    }
}
