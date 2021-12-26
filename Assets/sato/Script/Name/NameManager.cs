using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

    private static Hashtable playerProps = new Hashtable();

    private void Awake()
    {
        // カスタムプロパティのキーを設定(各プレイヤーが生成時にisReadyの変数を持っているイメージ
        playerProps["isPlayerReady"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        playerProps.Clear();
    }

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
        PhotonNetwork.LocalPlayer.NickName = text.text;

        GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(scene);
    }

    public bool DebugFlagName()
    {
        return debugFlag;
    }

    // プレイヤーがルームに入室時の処理
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
}
