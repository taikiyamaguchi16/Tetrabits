using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroTitle : MonoBehaviour
{
    [SerializeField] SceneObject nextScene;
    GameInGameSwitcher gameInGameSwitcherComponent;

    // Start is called before the first frame update
    void Start()
    {
        gameInGameSwitcherComponent = GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger())
        {
            gameInGameSwitcherComponent.SwitchGameInGameScene(nextScene);
        }
    }
}
