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
    [SerializeField] VisualEffect coolingCollisionEffect;
    [SerializeField] float spawnCount=1.0f;
    [SerializeField] float spawnCountFire=50.0f;

    //[SerializeField] VisualEffect coolingCollisionFireEffect;
    int coolingCollisionEffectFrameCount = 0;

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

        bool hitting = false;
        if (running)
        {
            //電池消費
            batteryHolder.ConsumptionOwnBattery(consumeBatteryPerSeconds * Time.deltaTime);

            bool setHitPoint = false;

            //レイとばして冷却ターゲット削除        
            //11/19 なんかforwardの逆にレイが飛んでるっぽい
            Debug.DrawRay(coolingMuzzule.position, -coolingMuzzule.forward * 10000.0f, Color.blue);
            var hits = Physics.RaycastAll(new Ray(coolingMuzzule.position, -coolingMuzzule.forward), 10000.0f);
            // 結果を発射元から距離が近い順でソート
            System.Array.Sort(hits, (a, b) => (int)Vector3.Distance(a.point, transform.position) - (int)Vector3.Distance(b.point, transform.position));
            foreach (var hit in hits)
            {
                //Debug.Log("hit!:" + hit.collider.gameObject.name);

                hitting = true;

                //エフェクトの移動
                if (!setHitPoint                                //このフレームでは未設定
                    && hit.collider.gameObject != gameObject    //自分自身でない
                    && (!hit.collider.isTrigger|| hit.collider.CompareTag("CoolingTarget")))                  //トリガーの当たり判定ではない                
                {
                    setHitPoint = true;
                    coolingCollisionEffect.transform.position = hit.point;
                    //coolingCollisionFireEffect.transform.position = hit.point;

                    coolingCollisionEffectFrameCount++;
                    if (coolingCollisionEffectFrameCount > 10)
                    {
                        coolingCollisionEffectFrameCount = 0;
                        coolingCollisionEffect.Play();
                        //coolingCollisionFireEffect.Play();
                    }
                }//Effect Play OK!

                //炎ターゲットに当たっているか
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

                    //coolingCollisionEffect.enabled=false;
                    //coolingCollisionFireEffect.enabled = true;
                    coolingCollisionEffect.SetFloat("spawn count", spawnCountFire);
                    //coolingtargetに一回でもあたったら終了
                    break;
                }
                else
                {
                    //coolingCollisionEffect.enabled = true;
                    //coolingCollisionFireEffect.enabled = false;
                    coolingCollisionEffect.SetFloat("spawn count", spawnCount);
                }
            }
        }//Runnning
        if (!hitting)
        {
            coolingCollisionEffect.Stop();
            //coolingCollisionFireEffect.Stop();
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
        if (runningRotator)
        {
            Debug.LogError("誰かが(多分自分)冷却起動中のため冷却装置を起動できません");
            return;
        }
        CoolerRotater cr;
        if (_desc.playerObj.TryGetComponent(out cr))
        {
            Debug.LogError("なぜか冷却装置回転用コンポーネントがすでについています　破壊します");
            Destroy(cr);
        }


        //おそらくIsMineで呼ばれてるので同期関数をそのまま呼び出す

        //Debug.Log("CoolingDeviceのStartPlayerActionが呼ばれました");
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
        runningRotator = null;
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
