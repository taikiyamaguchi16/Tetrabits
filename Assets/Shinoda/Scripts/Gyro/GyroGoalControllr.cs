using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GyroGoalControllr : MonoBehaviour
{
    [SerializeField] SceneObject nextScene = null;
    //GameInGameSwitcher gameInGameSwitcherComponent;
    bool loadable = true;

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
        else if (PhotonNetwork.IsMasterClient && loadable)
        {
            GameInGameUtil.SwitchGameInGameScene(nextScene);
            loadable = false;
        }
        //else gameInGameSwitcherComponent.SwitchGameInGameScene(nextScene);
    }
}
