using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInGameResultManager : MonoBehaviour
{
    [SerializeField] SceneObject nextScene;

    // Update is called once per frame
    void Update()
    {
        if (Photon.Pun.PhotonNetwork.IsMasterClient
            && (XInputManager.GetButtonTrigger(0, XButtonType.A) || Input.GetKeyDown(KeyCode.Return)))
        {
            //ルーム解散処理？

            var managerObj = GameObject.Find("GameMainManager");
            managerObj.GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(nextScene);
        }
    }
}
