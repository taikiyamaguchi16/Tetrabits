using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameInGameSwitcher : MonoBehaviour
{
    [SerializeField] SceneObject sceneObjA;
    [SerializeField] SceneObject sceneObjB;

    string currentGameInGameSceneName="";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchScene(sceneObjB);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchScene(sceneObjA);
        }
    }

    void SwitchScene(string _nextSceneName)
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
