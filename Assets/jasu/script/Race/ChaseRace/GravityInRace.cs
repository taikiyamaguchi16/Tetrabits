using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInRace : MonoBehaviour
{
    [SerializeField]
    AIGroundSensor groundSensor;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float gravity = -200f;

    private void Update()
    {
        if (groundSensor.GetOnGround())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }

    private void FixedUpdate()
    {
        if (!groundSensor.GetOnGround())
        {
            Vector3 gravityVec = Vector3.zero;
            gravityVec.y = gravity;
            rb.AddForce(gravityVec, ForceMode.Acceleration);
        }
    }
}
