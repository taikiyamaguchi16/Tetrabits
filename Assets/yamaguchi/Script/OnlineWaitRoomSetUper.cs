using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineWaitRoomSetUper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //カセット表示オン
        //とりあえずFindでテスト
        var cassetHolderObj = GameObject.Find("CassetHolder");
        if (!cassetHolderObj)
        {
            cassetHolderObj = GameObject.Find("cassette_socket2");
        }

        cassetHolderObj.GetComponent<CassetteManager>().AppearAllCassette();

        VirtualCameraManager.OnlyActive(1);
    }
}
