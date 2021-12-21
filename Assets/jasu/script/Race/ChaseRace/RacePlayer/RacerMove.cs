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

    [SerializeField, Tooltip("坂を上る時移動速度にかける倍率")]
    float moveSlopeMultiply = 0.5f;

    [SerializeField, Tooltip("泥の上の移動速度にかける倍率")]
    float moveDirtMultiply = 0.22f;

    [SerializeField, Tooltip("スタート時の慣性")]
    float moveForceMultiplyStart = 2f;

    [SerializeField, Tooltip("停止時の慣性")]
    float moveForceMultiplyStop = 1f;

    [SerializeField]
    float rotLate = 0.2f;

    public bool movable = true;

    Rigidbody rb;

    [Header("デバッグ")]

    [SerializeField]
    Vector3 moveVec;

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
    }

    private void FixedUpdate()
    {
        moveVec = Vector3.zero;

        // スロープ判定
        if (racerController.GetRacerGroundSensor().GetOnSlope())
        {
            moveVec = Vector3.ProjectOnPlane(Vector3.forward, racerController.GetRacerGroundSensor().groundNormalVec);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(moveVec), rotLate);

            moveVec.y *= (moveSpd / moveVec.z) * moveSlopeMultiply;
            moveVec.z = moveSpd * moveSlopeMultiply;
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, rotLate);
            moveVec.z = moveSpd;
        }

        // Dirt上
        if (racerController.GetRacerGroundSensor().GetOnDirt())
        {
            moveVec.z *= moveDirtMultiply;
        }

        // AddForce
        if (movable) // start
        {
            rb.AddForce(moveForceMultiplyStart * (moveVec - rb.velocity), ForceMode.Acceleration);
        }
        else // stop
        {
            rb.AddForce(moveForceMultiplyStop * (Vector3.zero - rb.velocity), ForceMode.Acceleration);
        }
    }
}
