using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateChase : AIState
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    PlayerClashSensor playerClashSensor;

    [SerializeField]
    RacerController racerController;

    [SerializeField]
    public int handleRacerSpdGear = 1;

    float[] racerSpdGears;

    [SerializeField]
    float warpDistance = 80f;

    [SerializeField]
    Vector3 warpOffset;

    [Header("デバッグ")]

    [SerializeField]
    Vector3 velocity;

    [SerializeField]
    float distance;

    private void Update()
    {
        velocity = rb.velocity;
    }

    public override void StateUpdate()
    {
        racerSpdGears = racerController.GetRacerMove().GetMoveSpdGears();

        distance = Mathf.Abs(racerController.transform.position.z - transform.position.z);

        if (distance > warpDistance)
        {
            Vector3 pos = racerController.transform.position;
            pos += warpOffset;
            transform.position = pos;
        }
    }

    public override void StateFixedUpdate()
    {
        // 移動
        if (!playerClashSensor.playerClash)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, racerSpdGears[handleRacerSpdGear]);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
    }
}
