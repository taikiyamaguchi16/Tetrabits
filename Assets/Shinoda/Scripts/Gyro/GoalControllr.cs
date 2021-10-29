using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalControllr : MonoBehaviour
{
    [SerializeField] SceneObject nextScene = null;
    //GameInGameSwitcher gameInGameSwitcherComponent;

    // Start is called before the first frame update
    void Start()
    {
        //gameInGameSwitcherComponent = GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nextScene == null) GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
        else GameInGameUtil.SwitchGameInGameScene(nextScene);
        //else gameInGameSwitcherComponent.SwitchGameInGameScene(nextScene);
    }
}
