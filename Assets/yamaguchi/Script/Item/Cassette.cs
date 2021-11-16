using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cassette : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Rigidbody rb;
    private Collider col;
    // Start is called before the first frame update
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;
    [SerializeField]
    private int priority;

    [SerializeField]
    SceneObject loadSceneObj;

    [SerializeField]
    bool isClear;

    private ItemPocket ownerSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
        isClear = false;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        if (photonView.IsMine)
        {
            if (!isOwned)
            {
                photonView.RPC(nameof(PickUp), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
            }
            else
                photonView.RPC(nameof(Dump), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return priority;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        if (photonView.IsMine)
        {
            return true;
        }
        return false;
    }

    public void CallPickUpCassette(int _id)
    {
        photonView.RPC(nameof(PickUp), RpcTarget.All, _id);
    }

    public void CallDumpCassette(int _id)
    {
        photonView.RPC(nameof(Dump), RpcTarget.All, _id);
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

    [PunRPC]
    public void PickUp(int _id)
    {

        GameObject _obj = NetworkObjContainer.NetworkObjDictionary[_id];

        //持たれたとき用の角度
        this.transform.rotation = Quaternion.Euler(90f, 0f, 180f);

        priority = 100;
        ownerSc = _obj.GetComponent<ItemPocket>();
        ownerSc.SetItem(this.gameObject);
        rb.isKinematic = true;
        col.enabled = false;
        this.transform.parent = _obj.transform;
        //保有状態に切り替え
        isOwned = true;
    }

    public SceneObject GetLoadSceneObj()
    {
        return loadSceneObj;
    }

    public void SetIsClearOn()
    {
        isClear = true;
    }

    public bool GetIsClear()
    {
        return isClear;
    }
}
