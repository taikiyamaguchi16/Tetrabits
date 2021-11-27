﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Battery : MonoBehaviourPunCallbacks, IPlayerAction
{
    [SerializeField]
    private GameObject energyGazeObj;
    private float energyGazeSize;
    private float energyGazePos;

    private float elpsedTime;

    private Rigidbody rb;
    private Collider col;
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;

    [SerializeField]
    private int priority;

    //電池残量
    [SerializeField, ReadOnly]
    private float level = 100f;

    private ItemPocket ownerSc;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
        level = 100f;
        elpsedTime = 0f;

        energyGazeSize = energyGazeObj.transform.localScale.y;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            elpsedTime += Time.deltaTime;
            //10秒に一回くらい同期取る
            if (elpsedTime > 10f)
            {
                Debug.Log("同期「");
                photonView.RPC(nameof(RPCSetBatteryLevel), RpcTarget.Others, level);
                elpsedTime = 0f;
            }

        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {

        if (!isOwned)
        {
            photonView.RPC(nameof(PickUp), RpcTarget.AllBufferedViaServer, _desc.playerObj.GetPhotonView().ViewID);
        }
        else
            photonView.RPC(nameof(Dump), RpcTarget.AllBufferedViaServer, _desc.playerObj.GetPhotonView().ViewID);

    }

    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return priority;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        return true;
    }

    public void CallPickUp(int _id)
    {
        photonView.RPC(nameof(PickUp), RpcTarget.AllBufferedViaServer, _id);
    }

    public void CallDump(int _id)
    {
        photonView.RPC(nameof(Dump), RpcTarget.AllBufferedViaServer, _id);
    }

    [PunRPC]
    public void PickUp(int _id)
    {
        GameObject _obj = NetworkObjContainer.NetworkObjDictionary[_id];
        priority = 100;
        ownerSc = _obj.GetComponent<ItemPocket>();
        ownerSc.SetItem(this.gameObject);
        rb.isKinematic = true;
        col.enabled = false;
        this.transform.parent = _obj.transform;
        //保有状態に切り替え
        isOwned = true;
    }

    [PunRPC]
    public void Dump(int _id)
    {
        GameObject _obj = NetworkObjContainer.NetworkObjDictionary[_id];
        if (_obj == ownerSc.gameObject)
        {
            ownerSc.SetItem(null);
            rb.isKinematic = false;
            col.enabled = true;
            this.transform.parent = null;
            isOwned = false;
            priority = 40;
        }
    }
    public float GetLevel()
    {
        return level;
    }

    public void BatteryConsumption(float _powerConsumption)
    {
        level -= _powerConsumption;
        //中のオブジェクトを残量に合わせて
        Vector3 keepSize = energyGazeObj.transform.localScale;
        //100は電池の最大容量のマジックナンバー
        keepSize.y = energyGazeSize * (level / 100f);

        energyGazeObj.transform.localScale = keepSize;
        if (level <= 0)
        {
            level = 0f;
            energyGazeObj.transform.localScale = Vector3.zero;
        }
       // photonView.RPC(nameof(RPCSetBatteryLevel), RpcTarget.Others, level);
    }

    [PunRPC]
    public void RPCSetBatteryLevel(float _level)
    {
        level = _level;
        //中のオブジェクトを残量に合わせて
        Vector3 keepSize = energyGazeObj.transform.localScale;
        //100は電池の最大容量のマジックナンバー
        keepSize.y = energyGazeSize * (level / 100f);

        energyGazeObj.transform.localScale = keepSize;
        if (level <= 0)
        {
            level = 0f;
            energyGazeObj.transform.localScale = Vector3.zero;
        }
    }
}
