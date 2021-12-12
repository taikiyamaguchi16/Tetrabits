using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;

public class BatteryDustBox : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Battery ownBattery;

    [SerializeField]
    VisualEffect batteryDustEfect;

    ItemPocket pocket;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
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
                ownBattery.CallDump(_desc.playerObj.GetPhotonView().ViewID);

                photonView.RPC(nameof(RPCDestroyBattery), RpcTarget.AllBufferedViaServer, ownBattery.photonView.ViewID);
                ownBattery = null;

                photonView.RPC(nameof(RPCNotifyThrowBatteryAway), RpcTarget.AllBufferedViaServer);
            }
            //バッテリーでなかった場合元に戻す
            else
            {
                ownBattery = checkbattery;
            }
        }
    }
    //12/8 田上　チュートリアルへバッテリーを捨てたことを通知する関数
    [PunRPC]
    void RPCNotifyThrowBatteryAway()
    {
        GameObject.Find("TutorialThrowBatteryAwayManager")?.GetComponent<TutorialThrowBatteryAwayManager>()?.NotifyThrowBatteryAway();
    }

    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 120;
    }

    //バッテリーの残量を返す
    public float GetBatterylevel()
    {
        if (ownBattery != null)
            return ownBattery.GetLevel();
        else
            return 0f;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        Battery checkbattery = ownBattery;
        //プレイヤーが何か持っていた場合
        if (otherPocket.GetItem() != null)
        {
            ownBattery = otherPocket.GetItem().GetComponent<Battery>();
            //渡されたのがバッテリーだった場合
            if (ownBattery == null)
            {
                return false;
            }
        }
        return true;
    }

    [PunRPC]
    public void RPCDestroyBattery(int _id)
    {
        batteryDustEfect.SendEvent("OnPlay");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(NetworkObjContainer.NetworkObjDictionary[_id]);
        }
    }
}
