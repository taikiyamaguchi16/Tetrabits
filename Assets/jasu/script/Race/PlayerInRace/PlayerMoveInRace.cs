using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveInRace : MonoBehaviour
{
    [SerializeField]
    public Rigidbody rb { get; private set; }

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    [SerializeField]
    BikeSlipDown bikeSlipDown = null;

    [SerializeField]
    DirtSplashSpawnInInput dirtSplashSpawnInInput;

    [SerializeField,Tooltip("移動速度")]
    float moveSpd = 10f;

    [SerializeField, Tooltip("坂を上る時移動速度にかける倍率")]
    float moveSlopeMultiply = 2f;

    [SerializeField,Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    float moveForceMultiplyStart = 5f;

    [SerializeField, Tooltip("移動速度の入力に対する追従度, 値が大きいとキビキビ動く")]
    float moveForceMultiplyStop = 5f;

    bool moveInput = false;

    [SerializeField]
    float gravity = -100f; // 重力

    Vector3 normalVec = Vector3.zero;

    [SerializeField]
    Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 移動入力
        moveInput = false;
        if (TetraInput.sTetraLever.GetPoweredOn() && !bikeSlipDown.isSliping)
        {
            moveInput = true;
        }
    }

    private void FixedUpdate()
    {
        //if (colliderSensorFront.GetExistInCollider() ||
        //    colliderSensorBack.GetExistInCollider())
        //{
        //    normalVec = Vector3.zero;
        //}

        // 移動
        //Vector3 moveVec = Vector3.zero;
        moveVec = Vector3.zero;
        if (moveInput)
        {
            //if (colliderSensorFront.GetExistInCollider() ||
            //    colliderSensorBack.GetExistInCollider())
            //moveVec = Vector3.forward * moveSpd;
            moveVec = Vector3.ProjectOnPlane(Vector3.forward, normalVec) * moveSpd;
            //moveVec.y = normalVec.y;
            //moveVec.z = 1f;
            //moveVec = new Vector3(0,normalVec.y, 1f) * moveSpd;
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

        dirtSplashSpawnInInput.playerMoveVec = moveVec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SlopeRoadInRace" ||
            collision.transform.parent.gameObject.tag == "SlopeRoadInRace")
        {
            normalVec = collision.contacts[0].normal;
            Debug.Log("坂の法線取得" + normalVec);
        }
        else if(collision.gameObject.tag == "FlatRoadInRace" ||
            collision.transform.parent.gameObject.tag == "FlatRoadInRace")
        {
            normalVec = Vector3.zero;
            Debug.Log("坂の法線リセット" + normalVec);
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
