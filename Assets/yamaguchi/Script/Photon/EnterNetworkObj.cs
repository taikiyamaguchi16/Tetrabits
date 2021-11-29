using Photon.Pun;
using UnityEngine;

public class EnterNetworkObj : MonoBehaviourPunCallbacks
{
    private bool isAddedNetworkObj = false;
    private void Awake()
    {
        NetworkObjContainer.NetworkObjDictionary.Add(photonView.ViewID, this.gameObject);
        isAddedNetworkObj = true;
    }

    public override void OnJoinedRoom()
    {
        if (!isAddedNetworkObj)
        {
            Debug.Log("ルーム参加時に追加されました");
            NetworkObjContainer.NetworkObjDictionary.Add(photonView.ViewID, this.gameObject);
        }
        isAddedNetworkObj = true;
    }
}