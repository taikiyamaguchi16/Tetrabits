using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInGameResultManager : MonoBehaviour
{
    [SerializeField] SceneObject nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool trigger = false;
        for (int i = 0; i < 4; i++)
        {
            if (XInputManager.GetButtonTrigger(i, XButtonType.A))
            {
                trigger = true;
                break;
            }
        }
        if (trigger)
        {
            var managerObj = GameObject.Find("GameMainManager");
            managerObj.GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(nextScene);

            Debug.LogWarning("Playerぶっころします");
            Destroy(GameObject.Find("Players"));
        }
    }
}
