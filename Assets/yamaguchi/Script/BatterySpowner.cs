﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatterySpowner : MonoBehaviourPunCallbacks, IPlayerAction
{
    //生成するオブジェクト
    [SerializeField]
    GameObject spownObj;
    //保有しているバッテリー
    private Battery ownBattery;
    //保有用のポケット
    ItemPocket pocket;
    //バッテリーの生成時間
    [SerializeField]
    float spownBatteryTime;
    //経過時間
    private float elpsedTime;

    [SerializeField]
    ParticleSystem smokeEfect;

    private bool canSpawn;
    // Start is called before the first frame update

    public void Start()
    {
        canSpawn = false;
    }
    public override void OnJoinedRoom()
    {
        pocket = GetComponent<ItemPocket>();
        //エフェクトの再生
        smokeEfect.Play();

        canSpawn = true;
    }

    private void Update()
    {
        if (ownBattery == null&&canSpawn)
        {
            elpsedTime += Time.deltaTime;
            if (spownBatteryTime < elpsedTime)
            {
                //エフェクトの再生
                smokeEfect.Play();

                elpsedTime = 0f;
            }
            //エフェクト再生して1秒たったら生成
            if (smokeEfect.time > 1f&&smokeEfect.isPlaying)
                SpawonBattery();
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        //エフェクト再生中には取れないように
        if (smokeEfect.isStopped)
        {
            ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
            //プレイヤーが何も持っていない場合
            if (otherPocket.GetItem() == null)
            {
                //バッテリーが生成されていた場合
                if (ownBattery != null)
                {
                    ownBattery.PickUp(_desc.playerObj);
                    pocket.SetItem(null);
                    ownBattery = null;
                }
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 100;
    }

    //バッテリーの残量を返す
    public float GetBatterylevel()
    {
        if (ownBattery != null)
            return ownBattery.GetLevel();
        else
            return 0f;
    }

    private void SpawonBattery()
    {
        //var b_obj = GameObject.Instantiate(spownObj);
        var b_obj = PhotonNetwork.Instantiate(spownObj.name, Vector3.zero, Quaternion.identity);
        ownBattery = b_obj.GetComponent<Battery>();
        ownBattery.PickUp(this.gameObject);
        elpsedTime = 0f;

        smokeEfect.time = 0f;
    }
}
