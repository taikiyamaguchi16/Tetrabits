using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    public bool isOffline;

    [SerializeField]
    bool networkDebugMode;

    public void Connect()
    {
        if (!isOffline)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("オフラインモード");
            PhotonNetwork.OfflineMode = true;
        }
    }

    public void Disconnect()
    {
        if (!isOffline)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            PhotonNetwork.OfflineMode = false;
        }
    }

    private void Start()
    {
        if (networkDebugMode)
        {
            if (isOffline)
            {
                Debug.Log("オフライン実行");
                PhotonNetwork.OfflineMode = true;
            }
            else
                PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isOffline)
        {          
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        }
        else
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        //PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }
}