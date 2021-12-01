using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class TetraButton : MonoBehaviourPunCallbacks
{
    [Header("Require Reference")]
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Prefab Reference")]
    [SerializeField] ButtonBodyCollider buttonBodyCollider;
    [SerializeField] Rigidbody buttonRb;
    [SerializeField] ConfigurableJoint foundationJoint;

    [Header("Status")]
    [SerializeField] float batteryConsumptionOnPressed = 1.0f;
    [SerializeField, Tooltip("ボタン押し返し力")] float pressedYSpring = 100.0f;
    [SerializeField] float stdYSpring = 5000.0f;
    [SerializeField, Tooltip("この値よりボタンと土台のY値の差分が低くなった時ボタンが押されます")] float pressableDifferenceY = 0;

    [Header("Indicator")]
    [SerializeField] EmissionIndicator emissionIndicator;

    [Header("Effect")]
    [SerializeField] VisualEffect smokeEffect;

    [System.NonSerialized] public bool deadBatteryDebug;

    protected bool buttonState;
    protected bool oldButtonState;

    // Update is called once per frame
    void Update()
    {
        //更新
        oldButtonState = buttonState;

        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || deadBatteryDebug)
        {
            if (PhotonNetwork.IsMasterClient)
            {//マスタークライアントでのみ処理を行う
                buttonState = (buttonRb.transform.localPosition.y - foundationJoint.transform.localPosition.y) < pressableDifferenceY;
                if (GetTrigger() || GetRelease())
                {//ON or OFF 時他クライアントに通知
                    CallSetButtonState(buttonState);
                }
            }

            //電力消費(GetTriggerが同期しているのでローカル処理でおｋ)
            if (GetTrigger())
            {
                smokeEffect.Play();
                batteryHolder.ConsumptionOwnBattery(batteryConsumptionOnPressed);
            }

            //エミッション処理
            if(buttonState)
            {
                emissionIndicator.SetColor(EmissionIndicator.ColorType.Using);
            }
            else
            {
                emissionIndicator.SetColor(EmissionIndicator.ColorType.Usable);
            }
        }
        else
        {
            //使用不可
            buttonState = false;
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Unusable);
        }

        //押し返し力の設定
        if (buttonBodyCollider.collisionNum > 0)
        {
            SetYDrive(pressedYSpring);
            buttonBodyCollider.triggerEnter = false;
        }
        else if (buttonBodyCollider.triggerEnter)
        {
            SetYDrive(stdYSpring);
        }
    }


    private void SetYDrive(float _spring)
    {
        if (foundationJoint)
        {
            var drive = foundationJoint.yDrive;
            drive.positionSpring = _spring;
            foundationJoint.yDrive = drive;
        }
    }

    void CallSetButtonState(bool _value)
    {
        photonView.RPC(nameof(RPCSetButtonState), RpcTarget.AllViaServer, _value);
    }
    [PunRPC]
    void RPCSetButtonState(bool _value)
    {
        buttonState = _value;
    }

    public bool GetPress() { return buttonState; }

    public bool GetTrigger() { return buttonState && !oldButtonState; }

    public bool GetRelease() { return !buttonState && oldButtonState; }
}
