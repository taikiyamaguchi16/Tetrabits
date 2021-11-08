using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameInGameUtil : MonoBehaviourPunCallbacks
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
            var photonView = managerObj.GetComponent<PhotonView>();
            var switcher = managerObj.GetComponent<GameInGameSwitcher>();

            if (photonView && switcher)
            {
                Debug.Log("RPC呼び出し：CallManagerSwitchGameInGameScene");
                photonView.RPC(nameof(CallManagerSwitchGameInGameScene), RpcTarget.All, switcher, _nextGameInGameScene);
            }
            else
            {
                Debug.LogError("photonViewかGameInGameSwitcherを取得できませんでした");
            }
        }
        else
        {
            Debug.LogError("GameMainManagerが見つかりませんでした");
        }
    }

    [PunRPC]
    private void CallManagerSwitchGameInGameScene(GameInGameSwitcher _switcher, string _nextSceneName)
    {
        _switcher.SwitchGameInGameScene(_nextSceneName);
    }
}
