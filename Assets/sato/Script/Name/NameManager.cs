using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("InputFieldを入れる")]
    GameObject inputField;

    [SerializeField]
    [Header("InputFieldの直下にあるtextを入れる")]
    Text text;

    [SerializeField]
    [Header("画面の案内オブジェクトを入れる")]
    GameObject TextObjects;

    [SerializeField]
    [Header("非表示にしたいキャンバスを入れる")]
    GameObject deactiveObject;

    [SerializeField]
    [Header("名前入力後に移動したいシーン")]
    SceneObject scene;

    [SerializeField]
    [Header("デバッグ用(trueで名前入力必須)")]
    bool debugFlag = false;

    Player player;
    Player[] players;

    static List<int> ActorNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //--------------------------------------------------
    // OnJoinedLobby
    // ロビーへの接続が成功した時に呼ばれるコールバック
    //--------------------------------------------------
    //public override void OnJoinedLobby()
    //{
    //    inputField.SetActive(true);

    //    text.SetActive(true);

    //    nameInput.SetActive(true);
    //}

    //--------------------------------------------------
    // OnJoinedRoom
    // ルームへの接続が成功した時に呼ばれるコールバック
    //--------------------------------------------------
    public override void OnJoinedRoom()
    {
        deactiveObject.SetActive(false);

        inputField.SetActive(true);

        TextObjects.SetActive(true);

        // ローカルプレイヤーとして登録(この段階で固有のものという認識でいいはず)
        player = PhotonNetwork.LocalPlayer;

        players = PhotonNetwork.PlayerList;

        // マスターがルームに入った時に初期化
        if (PhotonNetwork.IsMasterClient)
        {
            ActorNum = new List<int>();
        }
    }

    //--------------------------------------------------
    // OnLeftRoom
    // ルーム退出時のコールバック
    //--------------------------------------------------
    public override void OnLeftRoom()
    {
        deactiveObject.SetActive(true);

        inputField.SetActive(false);

        TextObjects.SetActive(false);

        Debug.Log("ルームから退出しました");
    }

    //--------------------------------------------------
    // NameInputExit
    // 名前確認承認時に実行
    //--------------------------------------------------
    public void NameInputExit()
    {
        // プレイヤー自身の名前を設定する
        player.NickName = text.text;

        //for (int i = 0; i <= players.Length; i++)
        //{
        //    if(i == player.ActorNumber)
        //    {
        //        // アクターナンバーと同じ番号でアクターの固有番号を格納
        //        ActorNum.Add(player.ActorNumber);
        //    }
        //}

        GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(scene);
    }

    public bool DebugFlagName()
    {
        return debugFlag;
    }

    // staticなアクター番号を格納している変数を返す
    static public List<int> GetActorNum()
    {
        return ActorNum;
    }
}
