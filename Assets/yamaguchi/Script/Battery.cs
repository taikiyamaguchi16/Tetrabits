using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Battery : MonoBehaviourPunCallbacks,IPlayerAction
{
    private Rigidbody rb;
    private BoxCollider col;
    // Start is called before the first frame update
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;
    [SerializeField]
    private int priority;
    //電池残量
    private float level;

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
