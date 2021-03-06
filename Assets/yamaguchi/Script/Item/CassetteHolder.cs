using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;

public class CassetteHolder : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Cassette ownCassette;

    private ItemPocket pocket;

    private GameInGameSwitcher sceneChanger;
    [SerializeField]
    VisualEffect inCassetteEfect;

    [SerializeField]
    CassetteManager cassetteManager;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
        sceneChanger = GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>();
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();

        if (otherPocket.GetItem() != null)
        {
            ownCassette = otherPocket.GetItem().GetComponent<Cassette>();
            //カセットで未クリアの場合セット
            if (ownCassette != null&&!ownCassette.GetIsClear())
            {
                //プレイヤーのゼンマイの消費のスタート
                photonView.RPC(nameof(ZenmaiDecreaseStart), RpcTarget.All);

                //カセットに設定されているシーンの読み込み
                sceneChanger.CallSwitchGameInGameScene(ownCassette.GetLoadSceneObj());

                ownCassette.CallDumpCassette(_desc.playerObj.GetPhotonView().ViewID);
                //プレイヤーのアイテムを取得してセット
                ownCassette.CallPickUpCassette(photonView.ViewID);

                otherPocket.SetItem(null);

                //カセットの表示を消す
                cassetteManager.CallHideAllCassette();
                //managerの現在のカセットを更新
                cassetteManager.SetActiveCassette(ownCassette);

                photonView.RPC(nameof(RPCPlayInCassetEfect), RpcTarget.All);
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 150;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();

        if (otherPocket.GetItem() != null)
        {
            ownCassette = otherPocket.GetItem().GetComponent<Cassette>();
            //カセットで未クリアの場合セット
            if (ownCassette != null && !ownCassette.GetIsClear())
            {
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    public void ZenmaiDecreaseStart()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(var pla in players)
        {
            pla.GetComponent<Zenmai>().decreaseTrigger = true;
            pla.GetComponent<CassetteTitleImage>().SetCassetteImageActive(false);
        }
    }

    [PunRPC]
    private void RPCPlayInCassetEfect()
    {
        inCassetteEfect.SendEvent("OnPlay");
    }
}
