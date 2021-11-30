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
            //12/1 田上　カセット湧かせる処理がチュートリアル後になったのでコメントアウト
            //var cassetHolderObj = GameObject.Find("CassetHolder");
            //if (!cassetHolderObj)
            //{
            //    cassetHolderObj = GameObject.Find("cassette_socket2");
            //}
            //cassetHolderObj.GetComponent<CassetteManager>().AppearAllCassette();

            //12/1 田上　全部StartUpGameMain関数に記述
            //var batterySpoawnerObj = GameObject.Find("BatterySpawner");
            //batterySpoawnerObj.GetComponent<BatterySpowner>().StartSpawn();
            //VirtualCameraManager.OnlyActive(1);

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
