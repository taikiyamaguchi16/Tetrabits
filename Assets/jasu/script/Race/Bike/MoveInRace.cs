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
    protected GroundSensor groundSensor = null;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    //[SerializeField]
    //protected DirtSplashSpawn dirtSplashSpawn;

    [SerializeField]
    protected AttitudeCtrlInRace attitudeCtrl;

    [SerializeField]
    protected OnDirt onDirt;

    [SerializeField]
    protected EffectGenerator effectGenerator;

    protected bool effectGenerated = false;

    [Header("パラメータ")]
    
    [SerializeField]
    protected float moveSpd;

    public float GetMoveSpd() { return moveSpd; }

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
    float distanceToGround = 5f;

    [SerializeField]
    protected float gravity = -100f; // 重力

    protected Vector3 groundNormalVec = Vector3.zero; // 地面の法線ベクトル

    [SerializeField]
    protected float onDirtSlowMultiply = 0.5f;

    public float moveSpdMultiply { get; set; } = 1f;

    [Header("デバッグ用")]

    [SerializeField]
    protected Vector3 moveVec = Vector3.zero;

    [SerializeField]
    protected bool onSlope = false;

    //[SerializeField]
    //protected int wheelonSlopeNum = 0; 

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

        if (onDirt.onDirt)
        {
            moveSpd *= onDirtSlowMultiply;
        }
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

    protected void CheckSlope()
    {
        onSlope = false;
        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distanceToGround))
        {
            if(hitInfo.transform.gameObject.tag == "SlopeRoadInRace" ||
            hitInfo.transform.parent.gameObject.tag == "SlopeRoadInRace")
            {
                onSlope = true;
                groundNormalVec = hitInfo.normal;
            }
        }
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

        if (!onSlope && groundSensor.GetSensorActive())
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

        moveSpd *= moveSpdMultiply;
    }

    protected void SetMoveVec()
    {
        // スロープチェック
        CheckSlope();

        if (!bikeSlipDown.isSliping)
        {
            if (onSlope)
            {
                if (!effectGenerated)
                {
                    effectGenerated = true;
                    effectGenerator.InstanceEffect();
                }

                moveVec = Vector3.ProjectOnPlane(Vector3.forward, groundNormalVec);

                // ありえん角度なら坂判定リセット
                //if (attitudeCtrl.CorrectAngle(Quaternion.LookRotation(moveVec).eulerAngles.x) < -90f)
                //{
                //    onSlope = false;
                //}

                transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.LookRotation(moveVec), rotLateSlope);
                if (attitudeCtrl.CorrectAngle(transform.parent.localRotation.eulerAngles.x) <= attitudeCtrl.CorrectAngle(Quaternion.LookRotation(moveVec).eulerAngles.x))
                {
                    transform.parent.localRotation = Quaternion.LookRotation(moveVec);
                }
                //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(moveVec), rotLateSlope);
                //if (attitudeCtrl.CorrectAngle(transform.localRotation.eulerAngles.x) <= attitudeCtrl.CorrectAngle(Quaternion.LookRotation(moveVec).eulerAngles.x))
                //{
                //    transform.localRotation = Quaternion.LookRotation(moveVec);
                //}

                moveVec.y *= (moveSpd / moveVec.z) * moveSlopeMultiply;
                moveVec.z = moveSpd * moveSlopeMultiply;
            }
            else
            {
                effectGenerated = false;

                groundNormalVec = Vector3.zero;

                //if (colliderSensor.GetExistInCollider())
                //{
                transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.identity, rotLateFlat);
                //}
                //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, rotLateFlat);
                moveVec.z = moveSpd;
            }
        }
    }

    // MoveVecのYに重力を掛ける
    protected void SetGravity()
    {
        // 空中でのみ重力
        if (!groundSensor.GetSensorActive())
        {
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

        //dirtSplashSpawn.moveVec = moveVec;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    WhenOnCollisionEnter(collision);
    //}

    //protected void WhenOnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "SlopeRoadInRace" ||
    //        collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
    //    {
    //        wheelonSlopeNum++;
    //        groundNormalVec = collision.contacts[0].normal;
    //        //Debug.Log("坂の法線取得" + groundNormalVec);
    //    }
    //    //else if ((collision.gameObject.tag == "FlatRoadInRace" ||
    //    //        collision.transform.parent.gameObject.tag == "FlatRoadInRace") && wheelonSlopeNum <= 0)
    //    //{
    //    //    groundNormalVec = Vector3.zero;
    //    //    //Debug.Log("坂の法線リセット" + groundNormalVec);
    //    //}
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    WhenOnCollisionExit(collision);
    //}

    //protected void WhenOnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "SlopeRoadInRace" ||
    //        collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
    //    {
    //        Vector3 rayPosition = transform.position;
    //        Ray ray = new Ray(rayPosition, Vector3.down);
    //        if (!Physics.Raycast(ray, distanceToGround))
    //        {
    //            wheelonSlopeNum--;
    //        }
    //    }
    //}
}
