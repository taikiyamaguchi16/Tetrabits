using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressedButtonSwitchScene : MonoBehaviour
{
    [SerializeField] Trisibo.SceneField nextScene;

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger())
        {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
            }
        }
    }
}
