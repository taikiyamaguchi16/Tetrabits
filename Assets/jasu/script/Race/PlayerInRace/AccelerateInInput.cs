using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateInInput : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb = null;

    [SerializeField]
    MoveInRace moveInRace;

    [SerializeField]
    EffectGenerator effectGenerator;

    //bool input = false;

    [Header("パラメータ")]

    [Header("加速")]

    //[SerializeField]
    //float acceleratePower = 300f;

    [SerializeField]
    float spdMultiply = 1.1f;

    [SerializeField]
    float accelSeconds = 2f;

    [SerializeField]
    float accelTimer = 0f;

    bool accelerating = false;

    [Header("体当たり")]

    [SerializeField]
    float bodyBlowActiveSeconds = 0.5f;

    float bodyBlowActiveTimer = 0f;

    public bool bodyBlowActive { get; private set; } = true;


    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //input = false;
        if (TetraInput.sTetraButton.GetTrigger())
        {
            moveInRace.moveSpdMultiply = spdMultiply;
            effectGenerator.InstanceEffect();

            accelerating = true;
            accelTimer = 0f;

            //input = true;
            bodyBlowActive = true;
        }

        if (accelerating)   // 加速中
        {
            accelTimer += Time.deltaTime;
            if(accelTimer > accelSeconds)
            {
                moveInRace.moveSpdMultiply = 1f;

                accelerating = false;
                accelTimer = 0f;
            }
        }

        if (bodyBlowActive)
        {
            bodyBlowActiveTimer += Time.deltaTime;
            if(bodyBlowActiveTimer > bodyBlowActiveSeconds)
            {
                bodyBlowActive = false;
                bodyBlowActiveTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        //if (input)
        //{
        //    input = false;
        //    //rb.AddForce(Vector3.forward * acceleratePower, ForceMode.Impulse);



        //    //effectGenerator.InstanceEffect();
        //    //GameInGameUtil.MoveGameObjectToOwnerScene(accelEffect, gameObject);
        //}
    }
}
