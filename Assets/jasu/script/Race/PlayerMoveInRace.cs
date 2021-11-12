using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveInRace : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    [SerializeField,Tooltip("移動速度")]
    float moveSpd = 10f;

    [SerializeField,Tooltip("移動速度の入力に対する追従度")]
    float moveForceMultiplier = 5f;

    bool moveInput = false;

    [SerializeField]
    float gravity = -100f; // 重力

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
        if ((colliderSensorFront.GetExistInCollider() ||
            colliderSensorBack.GetExistInCollider()) &&
            //TetraInput.sTetraButton.GetPress())
            TetraInput.sTetraLever.GetPoweredOn())
        {
            moveInput = true;
        }
    }

    private void FixedUpdate()
    {
        // 移動
        Vector3 moveVec = Vector3.zero;
        if (moveInput)
        {
            moveVec = transform.forward * moveSpd;
        }

        if(colliderSensorFront.GetExistInCollider() ||
            colliderSensorBack.GetExistInCollider())
        {
            rb.AddForce(moveForceMultiplier * (moveVec - rb.velocity), ForceMode.Acceleration);
        }

        // 重力
        rb.AddForce(new Vector3(0, gravity, 0));
    }
}
