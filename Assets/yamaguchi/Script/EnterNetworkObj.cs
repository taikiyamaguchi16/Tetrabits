using Photon.Pun;
using UnityEngine;

public class EnterNetworkObj : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        NetworkObjContainer.NetworkObjDictionary.Add(photonView.ViewID, this.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            foreach(var net in NetworkObjContainer.NetworkObjDictionary)
            {
                Debug.Log(net.Key + ":" + net.Value.name);
            }
        }
    }
}