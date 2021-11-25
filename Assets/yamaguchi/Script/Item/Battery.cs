using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Battery : MonoBehaviourPunCallbacks, IPlayerAction
{
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
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {

        if (!isOwned)
        {
            photonView.RPC(nameof(PickUp), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        }
        else
            photonView.RPC(nameof(Dump), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);

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
        photonView.RPC(nameof(PickUp), RpcTarget.All, _id);
    }

    public void CallDump(int _id)
    {
        photonView.RPC(nameof(Dump), RpcTarget.All, _id);
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
        if (level < 0)
            level = 0f;
    }
}
