using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolingTargetStatus : MonoBehaviour, ICool
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
            //モニターを回復
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.RepairMonitor(damageToMonitor);
                //死ぬ
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }

    //public bool TryToKill(float _damage)
    //{
    //    hp -= _damage;
    //    if (hp <= 0)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
