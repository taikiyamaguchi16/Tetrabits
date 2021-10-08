using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPhotonPool : MonoBehaviourPunCallbacks
{
    public List<GameObject> PrefabList;


    // ルームへの参加が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに参加");
        // "NetworkedObject"プレパブからネットワークオブジェクトを生成する
        PhotonNetwork.Instantiate("cap", Vector3.zero, Quaternion.identity);
    }
}