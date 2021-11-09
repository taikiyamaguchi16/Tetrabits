using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CassetteHolder : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Cassette ownCassette;

    private ItemPocket pocket;

    private GameInGameSwitcher sceneChanger;

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
                //カセットに設定されているシーンの読み込み
                sceneChanger.CallSwitchGameInGameScene(ownCassette.GetLoadSceneObj());
                //プレイヤーのアイテムを取得してセット

                ownCassette.CallPickUpCassette(photonView.ViewID);

                otherPocket.SetItem(null);

                //カセットの表示を消す
                cassetteManager.HideAllCassette();
                //managerの現在のカセットを更新
                cassetteManager.SetActiveCassette(ownCassette);
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 150;
    }


}
