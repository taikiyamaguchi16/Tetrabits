using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialThrowBatteryAwayManager : MonoBehaviour
{
    [Header("Clear")]
    [SerializeField] GameObject allOKObject;
    [SerializeField] float clearWaitSeconds = 3.0f;
    [SerializeField] Trisibo.SceneField nextScene;
    bool throwed;

    public void NotifyThrowBatteryAway()
    {
        if (!throwed)
        {
            StartCoroutine(CoNotifyThrowBatteryAway());
            throwed = true;
        }
    }

    IEnumerator CoNotifyThrowBatteryAway()
    {
        allOKObject.SetActive(true);

        yield return new WaitForSeconds(clearWaitSeconds);

        if(Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
        }
    }
}
