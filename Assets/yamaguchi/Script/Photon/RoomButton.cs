using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomButton : MonoBehaviour
{
    private const int MaxPlayers = 4;

    int roomPlayerCount;    //room人数

    [SerializeField]
    [Header("シーン名")]
    private SceneObject scene;

    [SerializeField]
    private Text label = default;

    private MatchmakingView matchmakingView;
    private Button button;

    public string RoomName { get; private set; }

    [SerializeField]
    GameObject NumPos;

    [SerializeField]
    Image None;
    [SerializeField]
    Image One;
    [SerializeField]
    Image Two;
    [SerializeField]
    Image Three;
    [SerializeField]
    Image Four;

    public void Init(MatchmakingView parentView, int roomId)
    {
        matchmakingView = parentView;
        RoomName = $"Room{roomId}";

        button = GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        //playerの人数に応じて人のUIを出す
        //roomPlayerCount;

        switch (roomPlayerCount)
        {
            case 0:
                NumPos.GetComponent<Image>().sprite = None.sprite;
                break;
            case 1:
                //[serializeField] Image で登録してるUIImageをオンにする
                //2,3,4をオフ

                NumPos.GetComponent<Image>().sprite = One.sprite;
                break;
            case 2:
                //[serializeField] Image で登録してるUIImageをオンにする
                //もう一個
                NumPos.GetComponent<Image>().sprite = Two.sprite;
                break;

            case 3:
                NumPos.GetComponent<Image>().sprite = Three.sprite;
                break;
                
            case 4:
                NumPos.GetComponent<Image>().sprite = Four.sprite;
                break;

            default:
                Debug.LogError("roomPlayerCountの数がバグってます");
                break;
        }

    }


    private void OnButtonClick()
    {
        if (roomPlayerCount < MaxPlayers)
        {
            // ルーム参加処理中は、全ての参加ボタンを押せないようにする
            matchmakingView.OnJoiningRoom();

            // ボタンに対応したルーム名のルームに参加する（ルームが存在しなければ作成してから参加する）
            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaxPlayers;
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);

            GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>().SwitchGameInGameScene(scene);
        }
        else
        {//ルーム入れないよ！
            //音鳴らす
        }
    }

    public void SetPlayerCount(int playerCount)
    {
        //保存しておく
        roomPlayerCount = playerCount;

        //UIの更新
        label.text = $"{playerCount} / {MaxPlayers}";

        // ルームが満員でない時のみ、ルーム参加ボタンを押せるようにする
        if (playerCount < MaxPlayers)
        {
            button.interactable = true;
        }
        // button.interactable = (playerCount < MaxPlayers);
    }
}