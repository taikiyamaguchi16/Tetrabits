using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameInGameUtil : MonoBehaviourPunCallbacks
{
    //void Hoge()
    //{
    //    photonView;
    //}

    public static void MoveGameObjectToOwnerScene(GameObject _go, GameObject _owner)
    {
        SceneManager.MoveGameObjectToScene(_go, _owner.scene);
    }

    public static void SwitchGameInGameScene(string _nextGameInGameScene)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(_nextGameInGameScene);
        }
        else
        {
            Debug.LogError("GameMainManagerが見つかりませんでした");
        }
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


    //[PunRPC]
    //private void CallManagerSwitchGameInGameScene(string _nextSceneName)
    //{
    //    var managerObj = GameObject.Find("GameMainManager");
    //    if (managerObj)
    //    {
    //        managerObj.GetComponent<GameInGameSwitcher>().SwitchGameInGameScene(_nextSceneName);
    //    }
    //    else
    //    {
    //        Debug.LogError("GameMainManagerが見つかりませんでした");
    //    }
    //}
}
