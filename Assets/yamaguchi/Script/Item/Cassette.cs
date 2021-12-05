using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Cassette : MonoBehaviourPunCallbacks, IPlayerAction
{
    [SerializeField]
    private GameObject rendererObj;
    [SerializeField]
    private Color cassetteColor;

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

    [SerializeField]
    private Text actionText;

    private ItemPocket ownerSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
        isClear = false;

        priority = 50;

        rendererObj.GetComponent<Renderer>().material.color = cassetteColor;
    }
    private void Update()
    {
        if (!isOwned)
        {
            actionText.text = "拾う";
        }
        else
        {
            actionText.text = "すてる";
        }
    }
    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        if (!isOwned)
        {
            photonView.RPC(nameof(PickUp), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        }
        else
        {
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
        return true;
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
