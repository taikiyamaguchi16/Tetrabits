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
            Debug.LogError("GameMainManagerが見つかりませんでした");
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
            Debug.LogError("GameMainManagerが見つかりませんでした");
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
}
