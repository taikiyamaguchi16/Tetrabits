using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateInInput : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb = null;

    [SerializeField]
    GameObject effectPrefab = null;

    [SerializeField]
    Transform effectInstanceTrans;

    [Header("パラメータ")]

    [SerializeField]
    float acceleratePower = 300f;

    [SerializeField]
    float bodyBlowActiveSeconds = 0.5f;

    float bodyBlowActiveTimer = 0f;

    bool input = false;

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
            input = true;
            bodyBlowActive = true;
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
        if (input)
        {
            input = false;
            rb.AddForce(Vector3.forward * acceleratePower, ForceMode.Impulse);
            GameObject accelEffect = Instantiate(effectPrefab, effectInstanceTrans);
            accelEffect.transform.position = effectInstanceTrans.position;
            accelEffect.transform.localScale = effectInstanceTrans.localScale;
            //GameInGameUtil.MoveGameObjectToOwnerScene(accelEffect, gameObject);
        }
    }
}
