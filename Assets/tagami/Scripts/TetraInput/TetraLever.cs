using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraLever : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject fulcrumObj;

    [Header("Option")]
    [SerializeField] float switchingTime = 1.0f;

    float switchTimer;
    Quaternion onQt;
    Quaternion offQt;

    bool leverState;
    bool oldLeverState;

    // Start is called before the first frame update
    void Start()
    {
        onQt = fulcrumObj.transform.rotation * Quaternion.AngleAxis(30.0f, fulcrumObj.transform.forward);
        offQt = fulcrumObj.transform.rotation * Quaternion.AngleAxis(-30.0f, fulcrumObj.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        //前回の情報を保存
        oldLeverState = leverState;

        //leverStateによって支点の回転を決める
        if (leverState) {
            switchTimer += Time.deltaTime;
            if (switchTimer >= switchingTime){
                switchTimer = switchingTime;
            }
        }
        else {
            switchTimer -= Time.deltaTime;
            if(switchTimer<=0) {
                switchTimer = 0;
            }
        }
        fulcrumObj.transform.rotation = Quaternion.Slerp(offQt,onQt,switchTimer/switchingTime);
    }

    [ContextMenu("Switch!")]
    void Switch() { leverState = !leverState; }

    public bool GetPoweredOn() { return leverState; }
}
