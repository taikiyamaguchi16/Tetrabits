using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMoveWhenGoal : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float moveForceMultiply = 0.5f;

    private void FixedUpdate()
    {
        rb.AddForce(moveForceMultiply *  -rb.velocity, ForceMode.Acceleration);
    }

    private void OnEnable()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
    }
}
