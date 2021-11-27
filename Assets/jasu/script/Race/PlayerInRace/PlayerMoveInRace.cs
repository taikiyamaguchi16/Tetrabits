using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveInRace : MoveInRace
{
    bool moveInput = false;

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
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            moveInput = true;
        }
    }

    private void FixedUpdate()
    {
        moveVec = Vector3.zero;

        // 移動ベクトル作成
        if (moveInput)
        {
            SetMoveVec();
        }

        // 空中でのy軸の加速を切る
        SetGravity();

        // 加速
        AccelerationToMoveVec();
    }

    private void OnCollisionEnter(Collision collision)
    {
        WhenOnCollisionEnter(collision);
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
            bikeSlipDown.SlipStart("small");
        }
    }
}
