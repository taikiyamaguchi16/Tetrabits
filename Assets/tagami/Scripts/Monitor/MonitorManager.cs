using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorManager : MonoBehaviour
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

    public void RepairMonitor(float _repairHp)
    {
        monitorHp += _repairHp;
        if (monitorHp >= monitorHpMax)
        {
            monitorHp = monitorHpMax;
        }
    }

    void DealDamage(string _damageId)
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
            Debug.LogError(_damageId + "：冷却用ダメージのPrefabを発見できませんでした");
            return;
        }

        //ダメージ発生 生成場所どーしよ
        var createdObj = Instantiate(prefab, new Vector3(Random.Range(-20, 20), Random.Range(5, 20), 44), Quaternion.identity);
        //生成登録
        createdCoolingTargets.Add(createdObj);

        //ダメージ処理
        monitorHp -= createdObj.GetComponent<CoolingTargetStatus>().damageToMonitor;

        //ダメージ段階進行処理
        if (monitorHp <= 0)
        {
            if (currentMonitorStatusIndex >= (monitorStatuses.Count - 1))
            {//ゲームオーバー処理
                Debug.Log("GameOver");
                gameInGameSwitcher.SwitchGameInGameScene("");
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
            foreach(var obj in createdCoolingTargets)
            {
                if(obj)
                {
                    Destroy(obj);
                }
            }
            createdCoolingTargets.Clear();
        }

    }

    //static
    static MonitorManager sMonitorManager;

    [System.Obsolete("DealDamageToMonitor(float) is deprecated, please use DealDamageToMonitor(string) instead.")]
    public static void DealDamageToMonitor(float _damage)
    {
    }

    public static void DealDamageToMonitor(string _damageId)
    {
        if (sMonitorManager)
        {
            sMonitorManager.DealDamage(_damageId);
        }
    }
}
