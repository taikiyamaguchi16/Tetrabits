using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInGameSwitcher : MonoBehaviour
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
                SwitchGameInGameScene(val.sceneObject);
            }
        }
    }

    public void SwitchGameInGameScene(string _nextSceneName)
    {
        //現在のシーンを削除
        if (currentGameInGameSceneName.Length > 0)
        {
            SceneManager.UnloadSceneAsync(currentGameInGameSceneName);
        }
        //次のシーンへ移行
        if (_nextSceneName.Length > 0)
        {
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
