using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerReadyManager : MonoBehaviourPunCallbacks
{
    private const int _PLAYER_UPPER_LIMIT = 4;

    [SerializeField]
    Text PushButtonText;

    [SerializeField]
    [Header("Ready音")]
    AudioClip readySe;

    bool isReadyMaster = false;
    private bool isReady;

    private static Hashtable playerProps = new Hashtable();

    bool isPlayerLimit = false;

    int controllerID = 0;

    void Awake()
    {
        // カスタムプロパティのキーを設定(各プレイヤーが生成時にisReadyの変数を持っているイメージ
        playerProps["isPlayerReady"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        playerProps.Clear();

        if (PhotonNetwork.IsMasterClient)
        {
            playerProps["isPlayerReady"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
            playerProps.Clear();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
//#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Return))
            {
                GameObject.Find("SetUp").GetComponent<OnlineWaitRoomSetUper>().OnlineGameStart();
            }
//#endif
        }
        // ゲスト
        else
        {
            if (!isReady)
            {
                if (Input.GetKeyDown(KeyCode.Return) || XInputManager.GetButtonTrigger(controllerID, XButtonType.B))
                {
                    playerProps["isPlayerReady"] = true;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
                    playerProps.Clear();

                    isReady = (PhotonNetwork.LocalPlayer.CustomProperties["isPlayerReady"] is bool);

                    photonView.RPC(nameof(SendIsReadyGuest), RpcTarget.All);
                }
            }
        }
    }

    bool PlayerReadyCheck()
    {
        // 各要素に対してbool状況を確認
        foreach (var props in PhotonNetwork.PlayerList)
        {
            // 全員レディー完了でtrueが返る
            if(props.CustomProperties["isPlayerReady"] is bool isPlayerReady)
            {
                if (!isPlayerReady)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // 何番目のプレイヤーかをホストに送信
    [PunRPC]
    public void SendIsReadyGuest()
    {
        SimpleAudioManager.PlayOneShot(readySe);
    }

    // プレイヤーがルームに入室時にリスト更新
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // マスターチェック
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 変更チェック(されてなければ返す)
        if (!changedProps.ContainsKey("isPlayerReady"))
        {
            return;
        }

        // 上限人数でレディチェック処理開始
        if (PhotonNetwork.CurrentRoom.PlayerCount >= _PLAYER_UPPER_LIMIT)
        {
            // 全員レディ完了でマスターがゲームを開始できるように
            if (!PlayerReadyCheck())
            {
                isReadyMaster = false;

                return;
            }
            else
            {
                isReadyMaster = true;
            }
        }
        else
        {
            return;
        }
    }
}
