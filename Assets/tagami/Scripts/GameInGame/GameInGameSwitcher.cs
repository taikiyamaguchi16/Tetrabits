using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameInGameSwitcher : MonoBehaviour
{
    [SerializeField] string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        //Scene読み込み
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
