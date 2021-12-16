using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

[DefaultExecutionOrder(-1)]
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

    [Header("Sound")]
    [SerializeField] SEAudioClip pressClip;

    [Header("Indicator")]
    [SerializeField] List<EmissionIndicator> emissionIndicators;

    [Header("Effect")]
    [SerializeField] VisualEffect smokeEffect;

    [System.NonSerialized] public bool deadBatteryDebug;

    protected bool buttonState;
    protected bool oldButtonState;
    bool stateChanged;

    bool localMasterButtonState;
    bool oldLocalMasterButtonState;

    int testCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if (stateChanged)
        {
            stateChanged = false;
        }
        else
        {//stateが変更された時に１フレームバッファを設ける 
            oldButtonState = buttonState;
        }

        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || deadBatteryDebug)
        {
            if (PhotonNetwork.IsMasterClient)
            {//マスタークライアントでのみ処理を行う   
                oldLocalMasterButtonState = localMasterButtonState;
                localMasterButtonState = (buttonRb.transform.localPosition.y - foundationJoint.transform.localPosition.y) < pressableDifferenceY;
            }

            if (GetTrigger())   //GetTriggerが同期しているためローカル処理で良い
            {
                smokeEffect.Play();
                batteryHolder.ConsumptionOwnBattery(batteryConsumptionOnPressed);
                testCounter++;
                //Debug.Log(testCounter + "回 button pressed!");
                SimpleAudioManager.PlayOneShot(pressClip);
            }

            //エミッション処理
            if (buttonState)
            {
                foreach (var emissionIndicator in emissionIndicators)
                    emissionIndicator.SetColor(EmissionIndicator.ColorType.Using);
            }
            else
            {
                foreach (var emissionIndicator in emissionIndicators)
                    emissionIndicator.SetColor(EmissionIndicator.ColorType.Usable);
            }
        }
        else
        {
            //使用不可
            if (PhotonNetwork.IsMasterClient)
            {
                localMasterButtonState = false;
            }

            foreach (var emissionIndicator in emissionIndicators)
                emissionIndicator.SetColor(EmissionIndicator.ColorType.Unusable);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (
                       (localMasterButtonState && !oldLocalMasterButtonState)    //Trigger条件
                       ||
                       (!localMasterButtonState && oldLocalMasterButtonState)    //Release条件
                       )
            {//ON or OFF 時、全クライアントに通知
                CallSetButtonState(localMasterButtonState);
            }
        }

        //押し返し力の設定
        if (buttonBodyCollider.collidingObjects.Count > 0)
        {
            SetYDrive(pressedYSpring);
            buttonBodyCollider.triggerEnter = false;
        }
        //誰も乗っていない＆Stopperのトリガー判定に触れている
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

    void CallSetButtonState(bool _newState)
    {
        photonView.RPC(nameof(RPCSetButtonState), RpcTarget.AllViaServer, _newState);
    }
    [PunRPC]
    void RPCSetButtonState(bool _newState)
    {
        buttonState = _newState;
        stateChanged = true;
    }

    public bool GetPress() { return buttonState; }

    public bool GetTrigger() { return buttonState && !oldButtonState; }

    public bool GetRelease() { return !buttonState && oldButtonState; }
}
