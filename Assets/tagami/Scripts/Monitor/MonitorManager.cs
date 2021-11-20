using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MonitorManager : MonoBehaviourPunCallbacks
{
    [Header("Required Reference")]
    [SerializeField] GameInGameSwitcher gameInGameSwitcher;

    [Header("Status")]
    [SerializeField] float monitorHpMax = 100;
    float monitorHp;


    [System.Serializable]
    struct MonitorStageStatus
    {
        public GameObject monitorModelObject;
    }
    [Header("MonitorStage")]
    [SerializeField] List<MonitorStageStatus> monitorStatuses;
    int currentMonitorStatusIndex = 0;

    [Header("Cooling Target Created Position")]
    [SerializeField] Transform displayTransform;

    [System.Serializable]
    struct KeyGameObject { public string key; public GameObject go; }
    [Header("Cooling Target Prefabs")]
    [SerializeField] List<KeyGameObject> coolingTargetPrefabs;
    List<GameObject> createdCoolingTargets;

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
        createdCoolingTargets = new List<GameObject>();
    }

    private void Update()
    {
        //スライダー？更新
        if (monitorHpBarSlider)
        {
            monitorHpBarSlider.value = (float)monitorHp / (float)monitorHpMax;
        }
    }

    public void RecoveryDestructionStage()
    {
        if (currentMonitorStatusIndex > 0)
        {
            //前の段階に戻す
            monitorStatuses[currentMonitorStatusIndex].monitorModelObject.SetActive(false);
            monitorStatuses[currentMonitorStatusIndex - 1].monitorModelObject.SetActive(true);

            //現在のIndexを更新
            currentMonitorStatusIndex--;
        }

        //HPを全回復
        monitorHp = monitorHpMax;
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
            );
    }

    [PunRPC]
    void RPCDealDamage(string _damageId, Vector3 _damagePosition)
    {
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

        //ダメージ発生 生成場所どーしよ
        var createdObj = Instantiate(prefab, _damagePosition, Quaternion.identity);
        //生成登録
        createdCoolingTargets.Add(createdObj);

        //ダメージ処理
        monitorHp -= createdObj.GetComponent<CoolingTargetStatus>().damageToMonitor;

        //ダメージ段階進行処理
        if (monitorHp <= 0)
        {
            if (currentMonitorStatusIndex >= (monitorStatuses.Count - 2))
            {//ゲームオーバー処理
                Debug.Log("GameOver");
                gameInGameSwitcher.CallSwitchGameInGameScene("");
                return;
            }

            //次の破壊段階へ進める 
            //現在のモデルを非アクティブにし、次の段階のモデルをアクティブにする
            monitorStatuses[currentMonitorStatusIndex].monitorModelObject.SetActive(false);
            monitorStatuses[currentMonitorStatusIndex + 1].monitorModelObject.SetActive(true);

            //現在のIndexを更新
            currentMonitorStatusIndex++;

            //HP全回復
            monitorHp = monitorHpMax;
            //冷却ターゲット消去
            foreach (var obj in createdCoolingTargets)
            {
                if (obj)
                {
                    Destroy(obj);
                }
            }
            createdCoolingTargets.Clear();
        }

    }

    //static
    static MonitorManager sMonitorManager;

    //[System.Obsolete("DealDamageToMonitor(float) is deprecated, please use DealDamageToMonitor(string) instead.")]
    //public static void DealDamageToMonitor(float _damage)
    //{
    //}

    public static void DealDamageToMonitor(string _damageId)
    {
        if (sMonitorManager)
        {
            sMonitorManager.CallDealDamage(_damageId);
        }
    }
}
