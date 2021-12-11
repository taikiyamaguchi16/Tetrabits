using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerReadyManager : MonoBehaviourPunCallbacks
{
    private const int _PLAYER_UPPER_LIMIT = 4;

    [SerializeField]
    Text PushButtonText;

    bool isReadyMaster = false;
    public List<bool> isReadyGuest = new List<bool>();
    public bool isReady;

    bool isPlayerLimit = false;

    int controllerID = 0;

    GameObject MasterPlayer;

    Player[] players;

    int ReadiedGuest = 0;

    int MineActorNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        // とりあえず50
        for (int i = 0; i < 50; i++)
        {
            isReadyGuest.Add(false);
        }

        ReadiedGuest = 0;

        players = PhotonNetwork.PlayerList;
    }

    // Update is called once per frame
    void Update()
    {
        EachPlayerBehaivorChanger();

        KeyInput();
    }

    // マスターかゲストかでUIやらなんやらの処理を変える
    void EachPlayerBehaivorChanger()
    {
        // マスター
        if (PhotonNetwork.IsMasterClient)
        {
            if (isReadyMaster)
            {
                PushButtonText.text = "BボタンかEnterでゲームスタート!";
            }
            else
            {
                PushButtonText.text = "他のプレイヤーがReadyするまでお待ちください!";
            }

            if (ReadiedGuest >= 3)
            {
                isReadyMaster = true;
            }
        }
        // ゲスト
        else
        {
            if (isReady)
            {
                PushButtonText.text = "ホストがゲームを開始するまでお待ちください!";
            }
            else
            {
                PushButtonText.text = "BボタンかEnterでReady!";
            }
        }
    }

    void KeyInput()
    {
        // マスター
        if (PhotonNetwork.IsMasterClient)
        {
            if (isReadyMaster)
            {
                if (Input.GetKeyDown(KeyCode.Return) || XInputManager.GetButtonTrigger(controllerID, XButtonType.B))
                {
                    GameObject.Find("SetUp").GetComponent<OnlineWaitRoomSetUper>().OnlineGameStart();
                }
            }

            // エディターでのみ使用可能
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Return))
            {
                GameObject.Find("SetUp").GetComponent<OnlineWaitRoomSetUper>().OnlineGameStart();
            }
#endif
        }
        // ゲスト
        else
        {
            if (!isReady)
            {
                if (Input.GetKeyDown(KeyCode.Return) || XInputManager.GetButtonTrigger(controllerID, XButtonType.B))
                {
                    isReady = true;
                    photonView.RPC(nameof(SendIsReadyGuest), RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    public void SendIsReadyGuest()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (photonView.IsMine)
            {
                MineActorNum = players[i].ActorNumber;
            }
        }
        isReadyGuest[MineActorNum] = true;
        ReadiedGuest++;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players = PhotonNetwork.PlayerList;
    }
}
