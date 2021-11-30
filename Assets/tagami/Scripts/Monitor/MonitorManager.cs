using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [System.Serializable]
    struct MonitorStageStatus
    {
        public GameObject monitorModelObject;
        public Color colorFilter;
    }
    [Header("MonitorStage")]
    [SerializeField] List<MonitorStageStatus> monitorStatuses;
    int currentMonitorStatusIndex = 0;

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

    [Header("Option")]
    [SerializeField] Slider monitorHpBarSlider;

    private void Awake()
    {
        if (sMonitorManager)
        {
            Debug.LogWarning("static変数の上書き登録を行います 場合によってはバグを引き起こす可能性があります");
        }

        sMonitorManager = this;

        //初期回復
        monitorHp = monitorHpMax;
        //List作成
        //createdCoolingTargets = new List<GameObject>();
    }

    private void Update()
    {
        //スライダー？更新
        if (monitorHpBarSlider)
        {
            monitorHpBarSlider.value = (float)monitorHp / (float)monitorHpMax;
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

        //コールバック
        monitorDamageEvent.Invoke();
    }

    [ContextMenu("NextStage")]
    private void NextDestructionStage()
    {
        if (currentMonitorStatusIndex >= (monitorStatuses.Count - 1))
        {
            Debug.Log("これ以上モニターの破壊段階を進めることはできません");
            Debug.Log("GameMainOverを確認");
            //ゲーム画面落としてゲームオーバーシーンへ遷移とか？
            gameInGameSwitcher.CallSwitchGameInGameScene("");
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

        //破片を降り注がせる
        if (PhotonNetwork.IsMasterClient)
        {
            for (int count = 0; count < 3; count++)
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

    public static void AddCoolingTargets(GameObject _go)
    {
        if (sMonitorManager)
        {
            sMonitorManager.createdCoolingTargets.Add(_go);
        }
    }
}
