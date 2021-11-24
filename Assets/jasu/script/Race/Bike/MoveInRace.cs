using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveInRace : MonoBehaviour
{
    [SerializeField]
    public Rigidbody rb { get; protected set; }

    [SerializeField]
    protected ColliderSensor colliderSensorFront = null;

    [SerializeField]
    protected ColliderSensor colliderSensorBack = null;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown = null;

    [SerializeField]
    protected DirtSplashSpawn dirtSplashSpawn;

    [SerializeField,Tooltip("移動速度")]
    protected float moveSpd = 10f;

    [SerializeField, Tooltip("坂を上る時移動速度にかける倍率")]
    protected float moveSlopeMultiply = 2f;

    [SerializeField,Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    protected float moveForceMultiplyStart = 5f;

    [SerializeField, Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    protected float moveForceMultiplyStop = 5f;

    [SerializeField]
    protected float gravity = -100f; // 重力

    protected Vector3 normalVec = Vector3.zero;

    [SerializeField]
    protected Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        // 移動
        //Vector3 moveVec = Vector3.zero;
        moveVec = Vector3.zero;

        if (!bikeSlipDown.isSliping)
        {
            moveVec = Vector3.ProjectOnPlane(Vector3.forward, normalVec) * moveSpd;
            if (moveVec.y > 1f)
            {
                moveVec *= 2f;
            }
        }

        // 空中でのy軸の加速を切る
        if (!colliderSensorFront.GetExistInCollider() &&
            !colliderSensorBack.GetExistInCollider())
        {
            moveVec.y = gravity;
        }
        

        // 加速
        if(moveVec.z <= 0f) // stop
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
        if(collision.gameObject.tag == "SlopeRoadInRace" ||
            collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
        {
            normalVec = collision.contacts[0].normal;
            //Debug.Log("坂の法線取得" + normalVec);
        }
        else if(collision.gameObject.tag == "FlatRoadInRace" ||
            collision.transform.parent.gameObject.tag == "FlatRoadInRace")
        {
            normalVec = Vector3.zero;
            //Debug.Log("坂の法線リセット" + normalVec);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<DirtSplash>() != null &&
            other.GetComponent<DirtSplash>().parentInstanceID == gameObject.GetInstanceID())
        {
            return;
        }

        if(other.gameObject.tag == "Slip")
        {
            bikeSlipDown.SlipStart();
        }
    }
}
