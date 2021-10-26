using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraLever : MonoBehaviour, IPlayerAction
{
    [Header("Reference")]
    [SerializeField] GameObject fulcrumObj;
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Option")]
    [SerializeField] float switchingTime = 1.0f;

    [System.NonSerialized]
    public bool keyDebug = false;

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

        if (!keyDebug && batteryHolder && batteryHolder.GetBatterylevel() <= 0)
        {
            leverState = false;
        }
        if (keyDebug && Input.GetKeyDown(KeyCode.Return))
        {
            Switch();
        }

        //leverStateによって支点の回転を決める
        if (leverState)
        {
            switchTimer += Time.deltaTime;
            if (switchTimer >= switchingTime)
            {
                switchTimer = switchingTime;
            }
        }
        else
        {
            switchTimer -= Time.deltaTime;
            if (switchTimer <= 0)
            {
                switchTimer = 0;
            }
        }
        fulcrumObj.transform.rotation = Quaternion.Slerp(offQt, onQt, switchTimer / switchingTime);
    }

    [ContextMenu("Switch!")]
    void Switch()
    {
        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0)||keyDebug)
        {
            leverState = !leverState;
        }
    }

    public bool GetPoweredOn() { return leverState; }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        Switch();
    }

    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 50;
    }
}
