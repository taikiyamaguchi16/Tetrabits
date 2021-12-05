using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public interface ICool
{
    void OnCooled(float _coolPower);
}

public class MonitorCooler : MonoBehaviourPunCallbacks, IPlayerAction
{



    [Header("Required Reference")]
    [SerializeField] MonitorManager monitorManager;

    [Header("Status")]
    [SerializeField] float damageToCoolingTargetPerSeconds = 1.0f;

    [Header("Effect")]
    [SerializeField] VisualEffect coolingEffect;
    [SerializeField] Transform coolingMuzzule;

    [Header("RotateTarget")]
    [SerializeField] Transform rotateXTransform;
    [SerializeField] Transform rotateYTransform;

    //最終回転Qt
    Quaternion endQtX;
    Quaternion endQtY;

    //int controlXinputIndex = 0;
    bool running = false;
    CoolerRotater runningRotator;

    [Header("Battery Holder")]
    [SerializeField] BatteryHolder batteryHolder;
    [SerializeField] float consumeBatteryPerSeconds = 1.0f;

    [Header("Indicator")]
    [SerializeField] List<EmissionIndicator> emissionIndicatorList;


    void Start()
    {
        endQtX = rotateXTransform.localRotation;
        endQtY = rotateYTransform.localRotation;

        coolingEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //電池なかったら落とす
        if (PhotonNetwork.IsMasterClient && running && batteryHolder.GetBatterylevel() <= 0)
        {
            CallSetRunning(false);
        }

        if (running)
        {
            batteryHolder.ConsumptionOwnBattery(consumeBatteryPerSeconds * Time.deltaTime);

            //レイとばして冷却ターゲット削除        
            //11/19 なんかforwardの逆にレイが飛んでるっぽい
            Debug.DrawRay(coolingMuzzule.position, -coolingMuzzule.forward * 10000.0f, Color.blue);
            foreach (var hit in Physics.RaycastAll(new Ray(coolingMuzzule.position, -coolingMuzzule.forward), 10000.0f))
            {
                Debug.Log("hit!:" + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("CoolingTarget"))
                {
                    //Interfaceを呼ぶ
                    ICool iCool;
                    if (hit.collider.gameObject.TryGetComponent(out iCool))
                    {
                        iCool.OnCooled(damageToCoolingTargetPerSeconds * Time.deltaTime);
                    }
                    else
                    {
                        Debug.LogError("CoolingTargetObjectにはICoolを継承したコンポーネントをアタッチしてください");
                    }
                    //targetに一回でもあたったら終了
                    break;
                }
            }

        }

        //回転処理
        rotateXTransform.localRotation = Quaternion.Slerp(rotateXTransform.localRotation, endQtX, 0.1f);
        rotateYTransform.localRotation = Quaternion.Slerp(rotateYTransform.localRotation, endQtY, 0.1f);

        //インジケーター
        if (batteryHolder && batteryHolder.GetBatterylevel() > 0)
        {
            if (running)
            {
                foreach (var indicator in emissionIndicatorList)
                    indicator.SetColor(EmissionIndicator.ColorType.Using);
            }
            else
            {
                foreach (var indicator in emissionIndicatorList)
                    indicator.SetColor(EmissionIndicator.ColorType.Usable);
            }
        }
        else
        {
            foreach (var indicator in emissionIndicatorList)
                indicator.SetColor(EmissionIndicator.ColorType.Unusable);
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        //おそらくIsMineで呼ばれてるので同期関数をそのまま呼び出す

        Debug.Log("CoolingDeviceのStartPlayerActionが呼ばれました");
        if (batteryHolder.GetBatterylevel() > 0)
        {
            CallSetRunning(true);
        }

        runningRotator = _desc.playerObj.AddComponent<CoolerRotater>();
        runningRotator.monitorCooler = this;    //Call関数を呼んでもらうため
    }

    public void EndPlayerAction(PlayerActionDesc _desc)
    {
        CallSetRunning(false);

        Destroy(runningRotator);
    }

    public int GetPriority()
    {
        return 50;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        return true;
    }

    void CallSetRunning(bool _value)
    {
        photonView.RPC(nameof(RPCSetRunning), RpcTarget.All, _value);
    }
    [PunRPC]
    void RPCSetRunning(bool _value)
    {
        running = _value;
        if (running)
        {
            coolingEffect.Play();
        }
        else
        {
            coolingEffect.Stop();
        }
    }

    public void CallMultiplyRotation(Quaternion _qtX, Quaternion _qtY)
    {
        photonView.RPC(nameof(RPCMultiplyRotation), RpcTarget.AllViaServer, _qtX, _qtY);
    }
    [PunRPC]
    void RPCMultiplyRotation(Quaternion _qtX, Quaternion _qtY)
    {
        endQtX *= _qtX;
        endQtY *= _qtY;
    }


}
