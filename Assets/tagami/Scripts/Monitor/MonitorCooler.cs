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
    [SerializeField] VisualEffect coolingLaserEffect;
    [SerializeField] Transform coolingMuzzule;
    [SerializeField] VisualEffect coolingCollisionEffect;
    [SerializeField] float coolingCollisionEffectSpawnCount = 1.0f;
    [SerializeField] float coolingCollisionEffectSpawnCountFire = 50.0f;

    //[SerializeField] VisualEffect coolingCollisionFireEffect;
    int coolingCollisionEffectFrameCount = 0;

    [Header("RotateTarget")]
    [SerializeField] Transform rotateXTransform;
    [SerializeField] Transform rotateYTransform;

    //最終回転Qt
    Quaternion endQtX;
    Quaternion endQtY;
    float rotateX;
    float rotateY;
    [SerializeField] float rotateXMax = 90.0f;
    [SerializeField] float rotateXMin = -90.0f;
    [SerializeField] float rotateYMax = 90.0f;
    [SerializeField] float rotateYMin = -90.0f;

    //int controlXinputIndex = 0;
    bool running = false;
    CoolerRotater runningRotator;

    [Header("Battery Holder")]
    [SerializeField] BatteryHolder batteryHolder;
    [SerializeField] float consumeBatteryPerSeconds = 1.0f;

    [Header("Indicator")]
    [SerializeField] List<EmissionIndicator> emissionIndicatorList;

    [Header("Sound")]
    [SerializeField] SEAudioClip hitClip;
    GameObject hitFireObjBuff;
    GameObject oldHitFireObjBuff;
    [SerializeField] AudioSource coolingLaserAudioSource;
    [SerializeField] float coolingLaserVolumeScale = 0.5f;
    bool coolingLaserAudioForcePlay;

    [Header("Debug")]
    [SerializeField] bool deadBatteryDebugLocal = false;

    void Start()
    {
        endQtX = rotateXTransform.localRotation;
        endQtY = rotateYTransform.localRotation;

        coolingLaserEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //電池なかったら落とす
        if (PhotonNetwork.IsMasterClient
            && running
            && (batteryHolder.GetBatterylevel() <= 0) && !deadBatteryDebugLocal)
        {
            CallSetRunning(false);
        }

        bool hitting = false;
        if (running)
        {
            //電池消費
            batteryHolder.ConsumptionOwnBattery(consumeBatteryPerSeconds * Time.deltaTime);

            bool setHitPoint = false;

            //初期化
            hitFireObjBuff = null;

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
                    && (!hit.collider.isTrigger || hit.collider.CompareTag("CoolingTarget")))                  //トリガーの当たり判定ではない                
                {
                    setHitPoint = true;
                    coolingCollisionEffect.transform.position = hit.point;


                    //Play()連打
                    coolingCollisionEffectFrameCount++;
                    if (coolingCollisionEffectFrameCount > 20)
                    {
                        coolingCollisionEffectFrameCount = 0;
                        StartCoroutine(CoVisualEffectLatePlayStop(coolingCollisionEffect, 1.0f, true));
                        //coolingCollisionEffect.Play();
                    }

                    if (hit.collider.CompareTag("CoolingTarget"))
                    {
                        coolingCollisionEffect.SetFloat("spawn count", coolingCollisionEffectSpawnCountFire);
                    }
                    else
                    {
                        coolingCollisionEffect.SetFloat("spawn count", coolingCollisionEffectSpawnCount);
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

                    hitFireObjBuff = hit.collider.gameObject;

                    //coolingtargetに一回でもあたったら終了
                    break;
                }
            }
        }//Runnning
        if (!hitting)
        {
            //coolingCollisionEffect.Stop();
            StartCoroutine(CoVisualEffectLatePlayStop(coolingCollisionEffect, 1.0f, false));
        }

        //回転処理
        rotateXTransform.localRotation = Quaternion.Slerp(rotateXTransform.localRotation, endQtX, 0.1f);
        rotateYTransform.localRotation = Quaternion.Slerp(rotateYTransform.localRotation, endQtY, 0.1f);

        //ヒット音
        if (hitFireObjBuff && hitFireObjBuff != oldHitFireObjBuff)
        {
            SimpleAudioManager.PlayOneShot(hitClip);
        }
        oldHitFireObjBuff = hitFireObjBuff;

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

    IEnumerator CoVisualEffectLatePlayStop(VisualEffect _effect, float _lateSeconds, bool _play)
    {
        yield return new WaitForSeconds(_lateSeconds);
        if (_play)
        {
            _effect.Play();
        }
        else
        {
            _effect.Stop();
        }
    }


    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        Debug.Log("PlayerActionStart");

        //return
        if (runningRotator)
        {
            Debug.LogError("誰かが(多分自分)冷却起動中のため冷却装置を起動できません");
            return;
        }
        if (running)
        {
            Debug.LogWarning("おそらく他の人が使用中のため起動できません");
            return;
        }

        //？
        CoolerRotater cr;
        if (_desc.playerObj.TryGetComponent(out cr))
        {
            Debug.LogError("なぜか冷却装置回転用コンポーネントがすでについています　破壊します");
            Destroy(cr);
        }



        //おそらくIsMineで呼ばれてるので同期関数をそのまま呼び出す

        //Debug.Log("CoolingDeviceのStartPlayerActionが呼ばれました");
        if (batteryHolder.GetBatterylevel() > 0 || deadBatteryDebugLocal)
        {
            CallSetRunning(true);
        }

        runningRotator = _desc.playerObj.AddComponent<CoolerRotater>();
        runningRotator.monitorCooler = this;    //Call関数を呼んでもらうため
    }

    public void EndPlayerAction(PlayerActionDesc _desc)
    {
        Debug.Log("PlayerActionEnd");

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
            coolingLaserEffect.Play();
            coolingLaserAudioForcePlay = true;
            coolingLaserAudioSource.volume = coolingLaserVolumeScale;
            coolingLaserAudioSource.Play();
        }
        else
        {
            coolingLaserEffect.Stop();
            StartCoroutine(CoCoolingLaserFadeOutStop());
        }
    }
    IEnumerator CoCoolingLaserFadeOutStop()
    {
        coolingLaserAudioForcePlay = false;
        while (!coolingLaserAudioForcePlay)
        {
            coolingLaserAudioSource.volume -= Time.deltaTime;
            if (coolingLaserAudioSource.volume <= 0)
            {
                coolingLaserAudioSource.volume = 0;
                break;
            }
            yield return null;
        }
    }

    public void CallMultiplyRotation(float _deltaRotX, float _deltaRotY)
    {
        photonView.RPC(nameof(RPCMultiplyRotation), RpcTarget.AllViaServer, _deltaRotX, _deltaRotY);
    }
    [PunRPC]
    void RPCMultiplyRotation(float _deltaRotX, float _deltaRotY)
    {
        //delta補正
        var judgeRotX = rotateX + _deltaRotX;
        if (rotateXMax < judgeRotX)
        {
            _deltaRotX = rotateXMax - rotateX;
        }
        else if (judgeRotX < rotateXMin)
        {
            _deltaRotX = rotateXMin - rotateX;
        }
        endQtX *= Quaternion.AngleAxis(_deltaRotX, Vector3.right);
        rotateX += _deltaRotX;

        var judgeRotY = rotateY + _deltaRotY;
        if (rotateYMax < judgeRotY)
        {
            _deltaRotY = rotateYMax - rotateY;
        }
        else if (judgeRotY < rotateYMin)
        {
            _deltaRotY = rotateYMin - rotateY;
        }
        endQtY *= Quaternion.AngleAxis(_deltaRotY, Vector3.up);
        rotateY += _deltaRotY;
    }


}
