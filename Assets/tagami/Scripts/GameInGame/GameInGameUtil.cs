using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInGameUtil : MonoBehaviour
{
    [SerializeField] bool isMasterClientOverride = true;

    void Start()
    {
        sIsMasterClient = isMasterClientOverride;
    }

    //**********************************************************
    //static

    private static bool sIsMasterClient;

    public static bool IsMasterClient()
    {
        return (PhotonNetwork.IsMasterClient || sIsMasterClient);
    }

    public static void MoveGameObjectToOwnerScene(GameObject _go, GameObject _owner)
    {
        SceneManager.MoveGameObjectToScene(_go, _owner.scene);
    }

    public static void SwitchGameInGameScene(string _nextGameInGameSceneName)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(_nextGameInGameSceneName);
        }
        else
        {
            Debug.LogWarning("GameMainManagerが見つからないので直接シーンを変更します");
            SceneManager.LoadScene(_nextGameInGameSceneName);
        }
    }

    public static void StartGameInGameTimer(string _gameInGameTag)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameMainManager>().StartGameInGameTimer(_gameInGameTag);
        }
        else
        {
            Debug.LogError("GameMainManagerが見つからないのでタイマーをStartできませんでした");
        }
    }

    public static void StopGameInGameTimer(string _gameInGameTag)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameMainManager>().StopGameInGameTimer(_gameInGameTag);
        }
        else
        {
            Debug.LogError("GameMainManagerが見つからないので、タイマーをストップできませんでした");
        }
    }


    public static string GetSceneNameByBuildIndex(int _index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(_index);
        return path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
    }



    public static void SwitchGameInGameSceneOffline(string _nextGameInGameScene)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(_nextGameInGameScene);
        }
        else
        {
            Debug.LogError("GameMainManagerが見つかりませんでした");
        }
    }

    public static void DisconnectAndReloadGameMain()
    {
        //コンテナをクリア
        NetworkObjContainer.NetworkObjDictionary.Clear();

        //ルーム解散処理？
        Photon.Pun.PhotonNetwork.LeaveRoom();
        Photon.Pun.PhotonNetwork.LeaveLobby();
        Photon.Pun.PhotonNetwork.Disconnect();


        //12/6 田上　初期化とか面倒すぎるのでコメントアウト
        //オフライン関数でタイトルへ戻る
        //GameInGameUtil.SwitchGameInGameSceneOffline(nextScene);


        //GameMainの再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static bool TryGetCassetteManager(out CassetteManager _cassetteManager)
    {
        _cassetteManager = null;

        var obj = GameObject.Find("CassetteHolder");
        if (!obj)
        {
            Debug.LogError("CassetteHolderオブジェクトが見つかりませんでした");
            return false;
        }

        var cassetteManager = obj.GetComponent<CassetteManager>();
        if (!cassetteManager)
        {
            Debug.LogError("CassetteManagerを取得できませんでした");
            return false;
        }

        _cassetteManager = cassetteManager;
        return true;
    }
}
