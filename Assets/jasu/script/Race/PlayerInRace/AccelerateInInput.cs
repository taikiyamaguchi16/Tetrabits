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
    
    bool input = false;

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
