using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

using Photon.Pun;
using UnityEngine.Events;

using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

public class MonitorManager : MonoBehaviourPunCallbacks
{
    [Header("Required Reference")]
    [SerializeField] GameInGameSwitcher gameInGameSwitcher;
    [SerializeField] GlobalVolumeController globalVolumeController;

    [Header("Status")]
    [SerializeField] float monitorHpMax = 100;
    float monitorHp;
    [SerializeField] VisualEffect hpEffect;
    int hpEffectRateMax = 1;
    [SerializeField] Gradient hpEffectColorGradient;

    [System.Serializable]
    struct MonitorStageStatus
    {
        public GameObject monitorModelObject;
        public Color colorFilter;
    }
    [Header("MonitorStage")]
    [SerializeField] List<MonitorStageStatus> monitorStatuses;
    int currentMonitorStatusIndex = 0;
    [SerializeField] List<VisualEffect> nextStageEffects;
    [SerializeField] float playNextStageEffectIntervalSeconds = 0.5f;

    [Header("Monitor Damage Callback")]
    [SerializeField] UnityEvent monitorDamageEvent;

    [Header("Cooling Target Created Position")]
    [SerializeField] Transform displayTransform;
    [SerializeField] Vector3 coolingTargetOffset;

    [System.Serializable]
    struct KeyGameObject { public string key; public GameObject go; }
    [Header("Cooling Target Prefabs")]
    [SerializeField] List<KeyGameObject> coolingTargetPrefabs;
    List<GameObject> createdCoolingTargets = new List<GameObject>();

    [Header("Stage Debris")]
    [SerializeField] List<StageDebris> stageDebrisList;
    int numCreateRandomDebris = 1;
    [SerializeField] float debrisGaugeMax = 10.0f;
    float debrisGauge;
    [SerializeField] Slider debrisGaugeSlider;
    [SerializeField] Text numDebrisText;


    private void Awake()
    {
        if (sMonitorManager)
        {
            Debug.LogWarning("static変数の上書き登録を行います 場合によってはバグを引き起こす可能性があります");
        }
        sMonitorManager = this;

        //初期回復
        monitorHp = monitorHpMax;

        //EffectRateの記録
        hpEffectRateMax = hpEffect.GetInt("Rate");
        //エフェクト初期カラー設定
        UpdateMonitorHpEffect();
    }

    private void Update()
    {
        if(numDebrisText)
        {
            numDebrisText.text = "破片落下数："+numCreateRandomDebris;
        }
    }


    public void CallRepairMonitor(float _repairHp)
    {
        photonView.RPC(nameof(RPCRepairMonitor), RpcTarget.AllViaServer, _repairHp);
    }

    [PunRPC]
    public void RPCRepairMonitor(float _repairHp)
    {
        monitorHp += _repairHp;
        if (monitorHp >= monitorHpMax)
        {
            monitorHp = monitorHpMax;
        }

        //エフェクト更新
        UpdateMonitorHpEffect();
    }

    void CallDealDamage(string _damageId)
    {
        photonView.RPC(nameof(RPCDealDamage), RpcTarget.AllViaServer, _damageId,
            new Vector3(displayTransform.position.x + Random.Range(-displayTransform.localScale.x, displayTransform.localScale.x),
            displayTransform.position.y + Random.Range(-displayTransform.localScale.y, displayTransform.localScale.y),
            displayTransform.position.z)
            + coolingTargetOffset
            );
    }

    [PunRPC]
    void RPCDealDamage(string _damageId, Vector3 _damagePosition)
    {
        //**********************************************************
        //冷却ターゲット生成
        GameObject prefab = null;
        foreach (var keyObj in coolingTargetPrefabs)
        {
            if (keyObj.key == _damageId)
            {
                prefab = keyObj.go;
            }
        }
        if (!prefab)
        {
            Debug.LogError("damageId:" + _damageId + "  用の冷却用ダメージPrefabは登録されていません");
            return;
        }
        //HPが0になるならそもそも破壊されるので生成しない
        var monitorHpBuff = monitorHp;
        monitorHpBuff -= prefab.GetComponent<CoolingTargetStatus>().damageToMonitor;
        //ダメージ発生 生成場所どーしよ
        if (PhotonNetwork.IsMasterClient && monitorHpBuff > 0)
        {
            PhotonNetwork.InstantiateRoomObject("GameMain/Monitor/" + prefab.name, _damagePosition, Quaternion.identity);
        }

        //**********************************************************
        //実際のダメージ処理
        monitorHp -= prefab.GetComponent<CoolingTargetStatus>().damageToMonitor;
        //ダメージ段階進行処理
        if (monitorHp <= 0)
        {
            NextDestructionStage();
        }

        //**********************************************************
        //破片処理
        debrisGauge += prefab.GetComponent<CoolingTargetStatus>().damageToMonitor;
        if (debrisGauge >= debrisGaugeMax)
        {//破片発生処理
            debrisGauge = 0.0f;

            if (PhotonNetwork.IsMasterClient)
            {
                CallScatterDebris(numCreateRandomDebris);
            }            
        }
        //Slider更新
        debrisGaugeSlider.maxValue = debrisGaugeMax;
        debrisGaugeSlider.value = debrisGauge;

        //**********************************************************

        //エフェクト更新
        UpdateMonitorHpEffect();

        //コールバック
        monitorDamageEvent.Invoke();
    }
    private void UpdateMonitorHpEffect()
    {
        //EffectのRate更新
        float rate = hpEffectRateMax * (monitorHp / monitorHpMax);
        hpEffect.SetInt("Rate", (int)rate);

        //Effectのグラデ更新
        var hpEffectGradient = hpEffect.GetGradient("Gradient");
        var colorKeys = hpEffectGradient.colorKeys;
        colorKeys[1].color = hpEffectColorGradient.Evaluate(monitorHp / monitorHpMax);
        hpEffectGradient.colorKeys = colorKeys;
        hpEffect.SetGradient("Gradient", hpEffectGradient);
    }


    [ContextMenu("NextStage")]
    private void NextDestructionStage()
    {
        if (currentMonitorStatusIndex >= (monitorStatuses.Count - 1))
        {
            Debug.Log("これ以上モニターの破壊段階を進めることはできません");
            Debug.Log("GameMainOverを確認");
            //ゲーム画面落としてゲームオーバーシーンへ遷移とか？
            gameInGameSwitcher.CallSwitchGameInGameScene("GameInGameResult");
            return;
        }

        //次の破壊段階へ進める 
        //現在のモデルを非アクティブにし、次の段階のモデルをアクティブにする
        monitorStatuses[currentMonitorStatusIndex].monitorModelObject.SetActive(false);
        monitorStatuses[currentMonitorStatusIndex + 1].monitorModelObject.SetActive(true);
        globalVolumeController.colorFilter = monitorStatuses[currentMonitorStatusIndex + 1].colorFilter;

        //現在のIndexを更新
        currentMonitorStatusIndex++;

        //HP全回復
        monitorHp = monitorHpMax;
        //冷却ターゲット消去
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var obj in createdCoolingTargets)
            {
                if (obj)
                {
                    PhotonNetwork.Destroy(obj);
                }
            }
        }

        //エフェクトを再生する
        StartCoroutine(CoPlayNextStageEffects());

        //破片を降り注がせる
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    CallScatterDebris(numCreateRandomDebris);
        //}
    }

    IEnumerator CoPlayNextStageEffects()
    {
        foreach (var effect in nextStageEffects)
        {
            effect.Play();
            yield return new WaitForSeconds(playNextStageEffectIntervalSeconds);
        }
    }

    void CallResetNumDebris()
    {
        photonView.RPC(nameof(RPCResetNumScatterDebris), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void RPCResetNumScatterDebris()
    {
        numCreateRandomDebris = 1;
    }

    void CallAddNumDebris()
    {
        photonView.RPC(nameof(RPCAddNumScatterDebris), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void RPCAddNumScatterDebris()
    {
        numCreateRandomDebris++;
        if (numCreateRandomDebris >= stageDebrisList.Count)
        {
            numCreateRandomDebris = stageDebrisList.Count;
        }
    }

    void CallScatterDebris(int _numDebris)
    {
        for (int count = 0; count < _numDebris; count++)
        {
            //空き検索
            List<int> usableIndexList = new List<int>();
            for (int i = 0; i < stageDebrisList.Count; i++)
            {
                if (!stageDebrisList[i].GetActiveSelf())
                {
                    usableIndexList.Add(i);
                }
            }
            //有効化
            if (usableIndexList.Count > 0)
            {
                var usableRandomIndex = usableIndexList[Random.Range(0, usableIndexList.Count)];
                stageDebrisList[usableRandomIndex].CallSetActive(true);
            }
        }//何回か行う
    }


    [ContextMenu("RecoveryStage")]
    private void RecoveryDestructionStage()
    {
        if (currentMonitorStatusIndex > 0)
        {
            //前の段階に戻す
            monitorStatuses[currentMonitorStatusIndex].monitorModelObject.SetActive(false);
            monitorStatuses[currentMonitorStatusIndex - 1].monitorModelObject.SetActive(true);
            globalVolumeController.colorFilter = monitorStatuses[currentMonitorStatusIndex - 1].colorFilter;


            //現在のIndexを更新
            currentMonitorStatusIndex--;
        }

        //HPを全回復
        monitorHp = monitorHpMax;
    }


    public void OnGUIWindow()
    {
        GUILayout.Label("====================");
        if (GUILayout.Button("NextStage"))
        {
            NextDestructionStage();
        }
        if (GUILayout.Button("RecoveryStage"))
        {
            RecoveryDestructionStage();
        }
    }



    //**********************************************************
    //static
    private static MonitorManager sMonitorManager;

    public static void DealDamageToMonitor(string _damageId)
    {
        if (sMonitorManager)
        {
            sMonitorManager.CallDealDamage(_damageId);
        }
    }

    public static void RepairMonitor(float _repairHp)
    {
        if (sMonitorManager)
        {
            sMonitorManager.CallRepairMonitor(_repairHp);
        }
    }

    public static void CallScatterDebrisInGameMainStage(int _numDebris)
    {
        if (sMonitorManager)
        {
            sMonitorManager.CallScatterDebris(_numDebris);
        }
    }

    public static void CallResetNumDebrisInGameMainStage()
    {
        sMonitorManager?.CallResetNumDebris();
    }

    public static void CallAddNumDebrisInGameMainStage()
    {
        sMonitorManager?.CallAddNumDebris();
    }

    //ほぼ専用関数
    //一気に破壊する用に登録してもらう
    public static void AddCoolingTargets(GameObject _go)
    {
        if (sMonitorManager)
        {
            sMonitorManager.createdCoolingTargets.Add(_go);
        }
    }


}
