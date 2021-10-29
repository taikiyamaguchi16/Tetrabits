using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInGameUtil
{
    public static void MoveGameObjectToOwnerScene(GameObject _go, GameObject _owner)
    {
        SceneManager.MoveGameObjectToScene(_go, _owner.scene);
    }

    public static void SwitchGameInGameScene(string _nextGameInGameScene)
    {
        var managerObj = GameObject.Find("GameMainManager");
        if (managerObj)
        {
            managerObj.GetComponent<GameInGameSwitcher>().SwitchGameInGameScene(_nextGameInGameScene);
        }
        else
        {
            Debug.LogWarning("GameMainManagerが見つかりませんでした");
        }
    }
}
