using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;
using UnityEngine.UI;

public class BatteryHolder : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Battery ownBattery;

    ItemPocket pocket;

    private  bool checkSetBatteryMaster;

    [SerializeField]
    List<VisualEffect> sparkEfects = new List<VisualEffect>();

    [SerializeField]
    Text actionText;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
        checkSetBatteryMaster = false;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {       
        photonView.RPC(nameof(RPCBatteryAction), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 110;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        Battery checkbattery = ownBattery;
        Battery possibleBattery;
        //プレイヤーが何か持っていた場合
        if (otherPocket.GetItem() != null)
        {
            possibleBattery = otherPocket.GetItem().GetComponent<Battery>();
            //渡されたのがバッテリーだった場合
            if (possibleBattery != null)
            {
                actionText.text = "入れる";

                //自分がバッテリを持っていた場合swapする
                if (checkbattery != null)
                {
                    actionText.text = "入れ替える";
                }
                return true;
            }
            //バッテリーでなかった場合元に戻す
            else
            {            
                return false;
            }
        }
        //何も持っていなかった場合自分のを渡す
        else
        {
            //自分がバッテリを持っていた場合渡す
            if (ownBattery != null)
            {
                actionText.text = "取り出す";
                return true;
            }
        }
        return false;
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
    private void RPCSetOwnBattery(int _id)
    {
        ownBattery = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<Battery>();

        PlaySparkEfect();
    }

    [PunRPC]
    private void RPCReleaseBattery()
    {
        pocket.SetItem(null);
        ownBattery = null;
    }

    public void ConsumptionOwnBattery(float _consumption)
    {
        if (ownBattery != null)
            ownBattery.BatteryConsumption(_consumption);
    }

    [PunRPC]
    private void RPCBatteryAction(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ItemPocket otherPocket = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<ItemPocket>();
            //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
            Battery checkbattery = ownBattery;
            //プレイヤーが何か持っていた場合
            if (otherPocket.GetItem() != null)
            {
                ownBattery = otherPocket.GetItem().GetComponent<Battery>();
                //渡されたのがバッテリーだった場合
                if (ownBattery != null)
                {
                    ownBattery.CallDump(_id);
                    ownBattery.CallPickUp(photonView.ViewID);
                    otherPocket.SetItem(null);
                    //自分がバッテリを持っていた場合swapする
                    if (checkbattery != null)
                    {
                        checkbattery.CallPickUp(_id);
                    }
                    //エフェクトの再生
                    PlaySparkEfect();
                    //他のプレイヤーのホルダーにもバッテリーをセット
                    photonView.RPC(nameof(RPCSetOwnBattery), RpcTarget.Others, ownBattery.photonView.ViewID);
                }
                //バッテリーでなかった場合元に戻す
                else
                {
                    ownBattery = checkbattery;
                }
            }
            //何も持っていなかった場合自分のを渡す
            else
            {
                //自分がバッテリを持っていた場合渡す
                if (ownBattery != null)
                {
                    ownBattery.CallPickUp(_id);

                    pocket.SetItem(null);
                    ownBattery = null;

                    //他のプレイヤーのホルダーのバッテリーを抜く
                    photonView.RPC(nameof(RPCReleaseBattery), RpcTarget.Others);
                }
            }
        }
    }

    private void PlaySparkEfect()
    {
        sparkEfects[Random.Range(0, 3)].SendEvent("OnPlay");
    }
}
