using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Battery : MonoBehaviourPunCallbacks,IPlayerAction
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
    
    //消費電力
    [SerializeField]
    float powerConsumption;

    private ItemPocket ownerSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;        
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        if (photonView.IsMine)
        {
            if (!isOwned)
            {
                PickUp(_desc.playerObj);
                //photonView.RPC(nameof(PickUp), RpcTarget.All);
            }
            else
                //photonView.RPC(nameof(Dump), RpcTarget.All, _desc.playerObj);
            Dump(_desc.playerObj);
        }
    }

    private void Update()
    {
        level -= powerConsumption * Time.deltaTime;
        if (level < 0)
            level = 0f;
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return priority;
    }

    [PunRPC]
    public void Dump(GameObject _obj)
    {
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

    [PunRPC]
    public void PickUp(GameObject _obj)
    {
        //持たれたとき用の角度
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        priority = 100;
        ownerSc = _obj.GetComponent<ItemPocket>();
        ownerSc.SetItem(this.gameObject);
        rb.isKinematic = true;
        col.enabled = false;
        this.transform.parent = _obj.transform;
        //保有状態に切り替え
        isOwned = true;
    }

    public float GetLevel()
    {
        return level;
    }
}
