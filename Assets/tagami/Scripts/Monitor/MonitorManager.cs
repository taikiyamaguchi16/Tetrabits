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
    struct MonitorStatus
    {
        public GameObject monitorModelObject;
    }
    [SerializeField] List<MonitorStatus> monitorStatuses;
    int currentMonitorStatusIndex = 0;


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

    public void RepairMonitor(float _healHp)
    {
        monitorHp += _healHp;
        if (monitorHp >= monitorHpMax)
        {
            monitorHp = monitorHpMax;
        }
    }

    void DealDamage(float _damage)
    {
        //ダメージ処理
        monitorHp -= _damage;
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

            //いくつかの決まった箇所に邪魔オブジェクトを配置

            //HP全回復
            monitorHp = monitorHpMax;
        }
    }

    //static
    static MonitorManager sMonitorManager;

    public static void DealDamageToMonitor(float _damage)
    {
        if (sMonitorManager)
        {
            sMonitorManager.DealDamage(_damage);
        }
    }
}
