using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameInGameSwitcher : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    struct SceneObjectAndKeyCode
    {
        public SceneObject sceneObject;
        public KeyCode keyCode;
    }
    [Header("Debug")]
    [SerializeField]
    List<SceneObjectAndKeyCode> gameInGameSwitchByKeys;

    //現在のゲーム内ゲーム名
    string currentGameInGameSceneName = "";

    // Update is called once per frame
    void Update()
    {
        foreach (var val in gameInGameSwitchByKeys)
        {
            if (Input.GetKeyDown(val.keyCode))
            {
                GameInGameUtil.SwitchGameInGameScene(val.sceneObject);
                //SwitchGameInGameScene(val.sceneObject);
            }
        }
    }

    //ラッピング
    public void CallSwitchGameInGameScene(string _nextSceneName)
    {
        photonView.RPC(nameof(SwitchGameInGameScene), RpcTarget.All, _nextSceneName);
    }

    [PunRPC]
    public void SwitchGameInGameScene(string _nextSceneName)
    {
        //現在のシーンを削除
        if (currentGameInGameSceneName.Length > 0)
        {
            Debug.Log(currentGameInGameSceneName + "：シーンを削除します");
            SceneManager.UnloadSceneAsync(currentGameInGameSceneName);
        }
        //次のシーンへ移行
        if (_nextSceneName.Length > 0)
        {
            Debug.Log(_nextSceneName + "：シーンへ移行します");
            SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("現在のシーン：" + currentGameInGameSceneName + "を削除しましたが、次のScene名が設定されていないので、移行できません");
        }

        //シーン名更新
        currentGameInGameSceneName = _nextSceneName;
    }
}
