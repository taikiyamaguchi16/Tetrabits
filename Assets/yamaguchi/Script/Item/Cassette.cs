using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Cassette : MonoBehaviourPunCallbacks, IPlayerAction
{
    [SerializeField]
    Sprite titleSprite;
    [SerializeField]
    Vector2 TitleImageSize;

    private CassetteTitleImage cassetteTitleImage;

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

        if(isClear)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        if (cassetteTitleImage != null)
        {
            cassetteTitleImage.SetCassetteImageActive(false);
            cassetteTitleImage = null;
        }
        photonView.RPC(nameof(RPCCassettePlayAction), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        //if (!isOwned)
        //{
        //    photonView.RPC(nameof(PickUp), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        //}
        //else
        //{
        //    photonView.RPC(nameof(Dump), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
        //}
    }

    [PunRPC]
    private void RPCCassettePlayAction(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isOwned)
            {
                photonView.RPC(nameof(PickUp), RpcTarget.AllBufferedViaServer, _id);
            }
            else
            {
                photonView.RPC(nameof(Dump), RpcTarget.AllBufferedViaServer, _id);
            }
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
        if (isOwned)
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
    }

    [PunRPC]
    public void PickUp(int _id)
    {
        if (!isOwned)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ItemPocket playerPocket = other.gameObject.GetComponent<ItemPocket>();
            //Playerが何も持っていない場合
            if (playerPocket.GetItem() == null)
            {
                cassetteTitleImage = other.gameObject.GetComponent<CassetteTitleImage>();
                cassetteTitleImage.SetCassetteImageActive(true);
                cassetteTitleImage.SetCassetteTexture(titleSprite, TitleImageSize);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (cassetteTitleImage != null)
            {
                cassetteTitleImage.SetCassetteImageActive(false);
                cassetteTitleImage = null;
            }
        }
    }
}
