using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveInRace : MoveInRace
{
    //bool moveInput = false;

    //[SerializeField]
    //List<int> slipObjList = new List<int>();

    [SerializeField]
    float onDirtMultiplyLever = 0.75f;

    //[SerializeField]
    //float spdPlayerNumMultiply = 0.2f;

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
        //moveInput = true;
        //if (TetraInput.sTetraLever.GetPoweredOn())
        //{
        //    moveInput = true;
        //}
    }

    private void LateUpdate()
    {
        // 移動速度決定
        //float moveSpdHolder = moveSpdStandard;
        //moveSpdStandard *= (1f + (spdPlayerNumMultiply * TetraInput.sTetraPad.GetNumOnPad())); // padの上に乗ってる分加速

        SetMoveSpd();
        //moveSpdStandard = moveSpdHolder;
    }

    private void FixedUpdate()
    {
        moveVec = Vector3.zero;

        // 移動ベクトル作成
        //if (!moveInput)
        //{
        //    moveSpd = 0f;
        //}

        if (onDirt.onDirt)
        {
            if (TetraInput.sTetraLever.GetPoweredOn())
            {
                moveSpd *= onDirtMultiplyLever;
            }
            else
            {
                moveSpd *= onDirtSlowMultiply;
            }
        }

        SetMoveVec();

        // 空中でのy軸の加速を切る
        SetGravity();

        // 加速
        AccelerationToMoveVec();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    WhenOnCollisionEnter(collision);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.GetComponent<DirtSplash>() != null &&
    //        other.GetComponent<DirtSplash>().parentInstanceID == gameObject.GetInstanceID())
    //    {
    //        return;
    //    }

    //    if(other.gameObject.tag == "Slip")
    //    {
    //        foreach(int objId in slipObjList)
    //        {
    //            if(objId == other.gameObject.GetInstanceID())
    //            {
    //                return;
    //            }
    //        }
    //        slipObjList.Add(other.gameObject.GetInstanceID());

    //        Debug.Log("ダメージ");
    //        bikeSlipDown.SlipStart("small");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    slipObjList.Remove(other.gameObject.GetInstanceID());
    //}
}
