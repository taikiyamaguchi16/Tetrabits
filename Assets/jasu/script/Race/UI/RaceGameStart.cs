using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceGameStart : MonoBehaviour
{
    [SerializeField]
    SceneObject firstStageScene = null;

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger() && firstStageScene != null)
        {
            GameInGameUtil.SwitchGameInGameScene(firstStageScene);
        }
    }
}
