using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class JumpMoveRemake : MonoBehaviourPunCallbacks
{
    Transform parent;
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
        parent = GameObject.Find("GameInGame").transform;
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
            photonView.RPC(nameof(JumpManEntry), RpcTarget.AllBufferedViaServer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            //player.transform.parent = parent;
            photonView.RPC(nameof(JumpManExit), RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    public void JumpManEntry()
    {
        player.transform.parent = this.transform;
    }

    [PunRPC]
    public void JumpManExit()
    {
        player.transform.parent = parent;
    }
}