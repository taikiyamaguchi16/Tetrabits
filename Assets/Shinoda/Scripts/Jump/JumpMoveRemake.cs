using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class JumpMoveRemake : MonoBehaviourPunCallbacks
{
    GameObject parent;
    GameObject player;
    GameObject playerFoot;

    Vector3 originPos;

    [SerializeField] float moveX;
    [SerializeField] float moveY;
    [SerializeField] float blockSize = 1f;
    Vector3 targetPos;

    [SerializeField] float moveTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("GameInGame");
        player = GameObject.Find("JumpMan");
        playerFoot = player.transform.Find("Foot").gameObject;

        originPos = transform.position;
        targetPos = new Vector3(originPos.x + (moveX * blockSize), originPos.y + (moveY * blockSize), originPos.z);
        // 移動床設定
        this.transform.DOMove(targetPos, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            //player.transform.parent = this.transform;
            photonView.RPC(nameof(PlayerEntry), RpcTarget.AllBufferedViaServer, player.GetPhotonView().ViewID, this.gameObject.GetPhotonView().ViewID);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            //player.transform.parent = parent;
            photonView.RPC(nameof(PlayerExit), RpcTarget.AllBufferedViaServer, player.GetPhotonView().ViewID, parent.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    public void PlayerEntry(int _playerID,int _thisID)
    {
        GameObject playerObj= NetworkObjContainer.NetworkObjDictionary[_playerID];
        GameObject thisObj = NetworkObjContainer.NetworkObjDictionary[_thisID];
        playerObj.transform.parent = thisObj.transform;
    }

    [PunRPC]
    public void PlayerExit(int _playerID,int _parentID)
    {
        GameObject playerObj = NetworkObjContainer.NetworkObjDictionary[_playerID];
        GameObject parentObj = NetworkObjContainer.NetworkObjDictionary[_parentID];
        playerObj.transform.parent = parentObj.transform;
    }
}