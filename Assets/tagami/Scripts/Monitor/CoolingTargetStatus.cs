using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class CoolingTargetStatus : MonoBehaviourPunCallbacks, ICool
{
    [Header("Status")]
    [SerializeField] float hpMax = 1.0f;
    float hp;

    [SerializeField] float DamageToMonitor;
    public float damageToMonitor { set { DamageToMonitor = value; } get { return DamageToMonitor; } }

    //[Header("Smoke")]
    //[SerializeField] VisualEffect smokeEffect;



    [Header("Option")]
    [SerializeField] UnityEngine.UI.Slider slider;
    [SerializeField] GameObject fireEffectObject;
    [SerializeField, Range(0, 1)] float fireEffectLocalScaleMinMultiplier = 0.3f;
    Vector3 fireEffectLocalScaleMax;

    bool isDead;
    //bool isCooled;
    //bool oldIsCooled;

    [Header("Sound")]
    [SerializeField] SEAudioClip popSEClip;

    private void Awake()
    {
        hp = hpMax;
        if (slider)
        {
            slider.maxValue = hpMax;
        }
        if (fireEffectObject)
        {
            fireEffectLocalScaleMax = fireEffectObject.transform.localScale;
        }

        //登録
        MonitorManager.AddCoolingTargets(gameObject);
    }

    void Start()
    {
        SimpleAudioManager.PlayOneShot(popSEClip);
        //ワールド空間に出す
        //smokeEffect.transform.parent = null;
    }

    private void Update()
    {
        if (slider)
        {
            slider.value = hp;
        }
        if (fireEffectObject)
        {
            fireEffectObject.transform.localScale = Vector3.Lerp(fireEffectLocalScaleMax * fireEffectLocalScaleMinMultiplier, fireEffectLocalScaleMax, hp / hpMax);
        }

        //if(isCooled&&!oldIsCooled)
        //{//Trigger
        //    smokeEffect.Play();
        //}
        //else if(!isCooled && oldIsCooled)
        //{
        //    smokeEffect.Stop();
        //}
        //oldIsCooled = isCooled;
        //isCooled = false;
    }

    public void OnCooled(float _damage)
    {
        //isCooled = true;

        hp -= _damage;
        if (hp <= 0 && !isDead)
        {
            isDead = true;

            //smokeEffect.GetComponent<DestroyOnTime>().enabled = true;

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
        if (obj)
        {
            obj.GetComponent<TutorialCoolingManager>().CompleteCooled();
        }
        else
        {
            Debug.Log("炎破壊時にTutorialCoolingManagerを探しましたが、見つかりませんでした");
        }
    }
}
