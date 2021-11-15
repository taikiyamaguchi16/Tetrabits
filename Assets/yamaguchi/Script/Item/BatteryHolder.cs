﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatteryHolder : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Battery ownBattery;

    ItemPocket pocket;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
    }

    private void Update()
    {
        if (ownBattery != null)
            ownBattery.BatteryConsumption();
    }

    public bool StartPlayerAction(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        Battery checkbattery = ownBattery;
        //プレイヤーが何か持っていた場合
        if (otherPocket.GetItem() != null)
        {
            ownBattery = otherPocket.GetItem().GetComponent<Battery>();
            //渡されたのがバッテリーだった場合
            if (ownBattery != null)
            {
                ownBattery.CallPickUp(photonView.ViewID);

                otherPocket.SetItem(null);
                //自分がバッテリを持っていた場合swapする
                if (checkbattery != null)
                {
                    ownBattery.CallPickUp(_desc.playerObj.GetPhotonView().ViewID);
                }
                //他のプレイヤーのホルダーにもバッテリーをセット
                photonView.RPC(nameof(RPCSetOwnBattery), RpcTarget.All);

                return true;
            }
            //バッテリーでなかった場合元に戻す
            else
            {
                ownBattery = checkbattery;
                return false;
            }
        }
        //何も持っていなかった場合自分のを渡す
        else
        {           
            //自分がバッテリを持っていた場合渡す
            if (ownBattery != null)
            {
                ownBattery.CallPickUp(_desc.playerObj.GetPhotonView().ViewID);

                pocket.SetItem(null);
                ownBattery = null;

                //他のプレイヤーのホルダーのバッテリーを抜く
                photonView.RPC(nameof(RPCReleaseBattery), RpcTarget.All);

                return true;
            }
        }
        return false;
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 110;
    }

    //バッテリーの残量を返す
    public float GetBatterylevel()
    {
        if (ownBattery != null)
            return ownBattery.GetLevel();
        else
            return 0f;
    }

    [PunRPC]
    private void RPCSetOwnBattery()
    {
        if (pocket.GetItem() != null)
            ownBattery = pocket.GetItem().GetComponent<Battery>();
    }

    [PunRPC]
    private void RPCReleaseBattery()
    {
        pocket.SetItem(null);
        ownBattery = null;
    }
}
