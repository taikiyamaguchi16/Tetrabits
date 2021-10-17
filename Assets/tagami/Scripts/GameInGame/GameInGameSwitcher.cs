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
    string currentGameInGameSceneName="";

    // Update is called once per frame
    void Update()
    {
        foreach(var val in gameInGameSwitchByKeys)
        {
            if(Input.GetKeyDown(val.keyCode))
            {
                SwitchGameInGameScene(val.sceneObject);
            }
        }
    }

    public void SwitchGameInGameScene(string _nextSceneName)
    {
        Debug.Log(currentGameInGameSceneName);
        if (currentGameInGameSceneName.Length > 0)
        {
            SceneManager.UnloadSceneAsync(currentGameInGameSceneName);
        }
        SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
        currentGameInGameSceneName = _nextSceneName;
    }
}
