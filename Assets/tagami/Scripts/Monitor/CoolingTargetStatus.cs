using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolingTargetStatus : MonoBehaviourPunCallbacks, ICool
{
    [Header("Status")]
    [SerializeField] float hpMax = 1.0f;
    float hp;

    [SerializeField] float DamageToMonitor;
    public float damageToMonitor { set { DamageToMonitor = value; } get { return DamageToMonitor; } }

    [Header("Option")]
    [SerializeField] UnityEngine.UI.Slider slider;

    bool isDead;

    private void Awake()
    {
        hp = hpMax;
        slider.maxValue = hpMax;

        //登録
        MonitorManager.AddCoolingTargets(gameObject);
    }

    private void Update()
    {
        slider.value = hp;
    }

    public void OnCooled(float _damage)
    {
        hp -= _damage;
        if (hp <= 0 && !isDead)
        {
            isDead = true;

            if (PhotonNetwork.IsMasterClient)
            {
                //モニターを回復
                MonitorManager.RepairMonitor(damageToMonitor);

                //チュートリアルに通知
                CallCompleteCooled();

                //死ぬ
                PhotonNetwork.Destroy(gameObject);
            }

        }
    }

    void CallCompleteCooled()
    {
        photonView.RPC(nameof(RPCCompleteCooled), RpcTarget.All);
    }
    [PunRPC]
    void RPCCompleteCooled()
    {
        var obj = GameObject.Find("TutorialCoolingManager");
        if(obj)
        {
            obj.GetComponent<TutorialCoolingManager>().CompleteCooled();
        }
        else
        {
            Debug.Log("炎破壊時にTutorialCoolingManagerを探しましたが、見つかりませんでした");
        }
    }
}
