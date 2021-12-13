using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class TetraLever : MonoBehaviourPunCallbacks, IPlayerAction
{
    [Header("Reference")]
    [SerializeField] GameObject fulcrumObj;
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Status")]
    [SerializeField] float batteryConsumptionPerSeconds = 1.0f;
    [SerializeField] float switchingTime = 1.0f;

    [Header("Sound")]
    [SerializeField] SEAudioClip leverClip;

    [Header("Indicator")]
    [SerializeField] EmissionIndicator emissionIndicator;

    [Header("Effect")]
    [SerializeField] VisualEffect sparkEffect;

    [System.NonSerialized]
    public bool deadBatteryDebug;

    float switchTimer;
    Quaternion onQt;
    Quaternion offQt;

    protected bool leverState;

    void Start()
    {
        if (fulcrumObj)
        {
            onQt = fulcrumObj.transform.rotation * Quaternion.AngleAxis(30.0f, fulcrumObj.transform.forward);
            offQt = fulcrumObj.transform.rotation * Quaternion.AngleAxis(-30.0f, fulcrumObj.transform.forward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //前回の情報を保存
        //oldLeverState = leverState;

        //バッテリーがないなら強制的にオフ
        if (!deadBatteryDebug && batteryHolder && batteryHolder.GetBatterylevel() <= 0)
        {
            leverState = false;
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Unusable);
        }
        //電力消費
        else if (leverState)
        {//使用中
            batteryHolder.ConsumptionOwnBattery(batteryConsumptionPerSeconds * Time.deltaTime);
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Using);
        }
        else
        {//使用可能
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Usable);
        }

        //支点回転
        if (fulcrumObj)
        {
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
    }

    private void CallSwitch()
    {
        photonView.RPC(nameof(RPCSwitch), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCSwitch()
    {
        //バッテリーがあるなら起動OK
        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || deadBatteryDebug)
        {
            leverState = !leverState;

            SimpleAudioManager.PlayOneShot(leverClip);

            if (leverState)
            {
                sparkEffect.Play();
                
            }
        }
    }

    public bool GetPoweredOn() { return leverState; }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        CallSwitch();
    }

    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 50;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        return true;
    }
}
