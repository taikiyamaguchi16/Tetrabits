using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerMove : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    public float moveSpd { get; private set; }

    [SerializeField]
    float[] moveSpdGears;

    public float[] GetMoveSpdGears()
    {
        return moveSpdGears;
    }

    public float GetMoveSpdGear(int _index)
    {
        return moveSpdGears[_index];
    }

    public int moveGear { get; private set; }

    public void SetMoveSpd(int _index)
    {
        moveGear = _index;
        moveSpd = moveSpdGears[moveGear];
    }

    [SerializeField, Tooltip("坂を上る時移動速度")]
    float moveSpdSlope = 80f;

    [SerializeField, Tooltip("泥の上の移動速度")]
    float moveSpdDirt = 80f;

    [SerializeField, Tooltip("スタート時の慣性")]
    float moveForceMultiplyStart = 2f;

    [SerializeField, Tooltip("停止時の慣性")]
    float moveForceMultiplyStop = 1f;

    [SerializeField]
    float rotLate = 0.2f;

    public bool movable = true;

    Rigidbody rb;

    //[SerializeField]
    //float gravity = -200f;

    [SerializeField]
    EffectGenerator slopeBoostEffect;

    bool slopeBoostSEplayable = true;

    [Header("デバッグ")]

    [SerializeField]
    Vector3 moveVec;

    [SerializeField]
    Vector3 velocity;

    [SerializeField]
    bool slopeBoost = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = racerController.GetRigidbody();

        moveSpd = moveSpdGears[0];
    }

    // Update is called once per frame
    void Update()
    {
        // 速度セット
        int numOnPad = TetraInput.sTetraPad.GetNumOnPad();
        if (numOnPad < 0) numOnPad = 0;
        if(numOnPad < moveSpdGears.Length)
        {
            SetMoveSpd(numOnPad);
        }
        else
        {
            SetMoveSpd(moveSpdGears.Length - 1);
        }

        velocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        moveVec = rb.velocity;

        if (racerController.GetRacerGroundSensor().GetOnGround())
        {
            slopeBoost = false;
        }

        // スロープ判定
        if (racerController.GetRacerGroundSensor().GetOnSlope())
        {
            moveVec = Vector3.ProjectOnPlane(Vector3.forward, racerController.GetRacerGroundSensor().groundNormalVec);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(moveVec), rotLate);

            moveVec.y *= (moveSpdSlope / moveVec.z);
            moveVec.z = moveSpdSlope;

            if (slopeBoostSEplayable)
            {
                slopeBoostSEplayable = false;
                slopeBoostEffect.InstanceEffect();
            }

            slopeBoost = true;
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, rotLate);
            moveVec.z = moveSpd;

            slopeBoostSEplayable = true;
        }

        // Dirt上
        if (racerController.GetRacerGroundSensor().GetOnDirt())
        {
            moveVec.z = moveSpdDirt;
        }

        if (slopeBoost)
        {
            moveVec.z = moveSpdSlope;
        }

        // AddForce
        if (movable) // start
        {
            //rb.AddForce(moveForceMultiplyStart * (moveVec - rb.velocity), ForceMode.Acceleration);
            rb.velocity = moveVec;
        }
        else // stop
        {
            rb.AddForce(moveForceMultiplyStop * (Vector3.zero - rb.velocity), ForceMode.Acceleration);
        }
    }
}
