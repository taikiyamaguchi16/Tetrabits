using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineWaitRoomSetUper : MonoBehaviourPunCallbacks
{
    //仮のフラグ
    private bool kariFg = false;
    // Start is called before the first frame update
    public void OnlineGameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(CallStartGameOnline), RpcTarget.All);
            //ルームを入室不可に
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    [PunRPC]
    public void CallStartGameOnline()
    {
        if (!kariFg)
        {
            //カセット表示オン
            //とりあえずFindでテスト
            var cassetHolderObj = GameObject.Find("CassetHolder");
            if (!cassetHolderObj)
            {
                cassetHolderObj = GameObject.Find("cassette_socket2");
            }

            var batterySpoawnerObj = GameObject.Find("BatterySpawner");
            cassetHolderObj.GetComponent<CassetteManager>().AppearAllCassette();

            batterySpoawnerObj.GetComponent<BatterySpowner>().StartSpawn();

            VirtualCameraManager.OnlyActive(1);

            //11/26 田上　カメラ引く処理の通知
            var gameMainManagerObject = GameObject.Find("GameMainManager");
            if (gameMainManagerObject)
            {
                gameMainManagerObject.GetComponent<GameMainStartUp>()?.StartUpGameMain();
            }
            else
            {
                throw new System.NullReferenceException(nameof(gameMainManagerObject));
            }

            kariFg = true;
        }
    }
}
