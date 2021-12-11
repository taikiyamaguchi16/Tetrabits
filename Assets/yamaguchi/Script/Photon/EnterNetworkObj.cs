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
            NetworkObjContainer.NetworkObjDictionary.Add(photonView.ViewID, this.gameObject);
        }
        isAddedNetworkObj = true;
    }
}