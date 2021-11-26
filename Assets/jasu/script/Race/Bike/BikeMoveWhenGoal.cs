using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMoveWhenGoal : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float moveForceMultiply = 0.5f;

    [SerializeField]
    protected float gravity = -100f; // 重力

    private void FixedUpdate()
    {
        Vector3 moveVec = Vector3.zero;
        moveVec.y = gravity;
        rb.AddForce(moveForceMultiply * (moveVec - rb.velocity), ForceMode.Acceleration);
    }

    private void OnEnable()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
    }
}
