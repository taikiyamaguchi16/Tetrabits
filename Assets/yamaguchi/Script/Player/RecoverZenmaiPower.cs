using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RecoverZenmaiPower : MonoBehaviourPunCallbacks, IPlayerAction
{
    Zenmai zenmai;  // ゼンマイ

    [SerializeField]
    float recoveryAmount = 0.02f;  // ゼンマイパワー回復量

    [SerializeField]
    PlayerMove movePlayer;

    [SerializeField]
    ZenmaiRotation zenmaiRot;

    bool windFlag = false;  // 巻きフラグ

    // Start is called before the first frame update
    void Start()
    {
        zenmai = GetComponent<Zenmai>();
    }

    private void Update()
    {
        if (windFlag&&photonView.IsMine)   // ゼンマイを巻く
        {
            zenmai.zenmaiPower += recoveryAmount*Time.deltaTime;
            if (zenmai.zenmaiPower > zenmai.maxZenmaiPower)
                zenmai.zenmaiPower = zenmai.maxZenmaiPower;
        }
    }

    // アクションインターフェース　スタート
    void IPlayerAction.StartPlayerAction(PlayerActionDesc _desc)
    {
        photonView.RPC(nameof(RPCRecoverZenmaiPower), RpcTarget.All, photonView.ViewID);

        _desc.playerObj.GetComponent<PlayerMove>().SetPlayerMovable(false);
    }

    // アクションインターフェース　エンド
    void IPlayerAction.EndPlayerAction(PlayerActionDesc _desc)
    {
        photonView.RPC(nameof(RPCEndRecoverZenmaiPower), RpcTarget.All, photonView.ViewID);
        _desc.playerObj.GetComponent<PlayerMove>().SetPlayerMovable(true);
    }

    int IPlayerAction.GetPriority()
    {
        return 50;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        return true;
    }

    [PunRPC]
    public void RPCRecoverZenmaiPower(int _id)
    {
        if(photonView.IsMine)
        {
            if(photonView.ViewID==_id)
            {
                windFlag = true;
                zenmai.decreaseTrigger = false;

                movePlayer.SetPlayerMovable(false);
                zenmaiRot.SetRecoverFg(true);
            }
        }
    }

    [PunRPC]
    public void RPCEndRecoverZenmaiPower(int _id)
    {
        if (photonView.IsMine)
        {
            if (photonView.ViewID == _id)
            {
                windFlag = false;
                zenmai.decreaseTrigger = true;

                movePlayer.SetPlayerMovable(true);
                zenmaiRot.SetRecoverFg(false);
            }
        }
    }
}
