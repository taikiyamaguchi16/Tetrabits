using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineWaitRoomSetUper : MonoBehaviour
{

    // Start is called before the first frame update
    public void OnlineGameStart()
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

        //ルームを入室不可に
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
}
