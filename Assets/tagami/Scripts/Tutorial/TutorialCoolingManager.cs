using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialCoolingManager : MonoBehaviour
{
    [SerializeField] Trisibo.SceneField nextScene;
    [SerializeField] GameObject allOKObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoCreateCoolingTarget());

    }
    IEnumerator CoCreateCoolingTarget()
    {
        yield return new WaitForSeconds(3);

        //Tutorial用の炎を出す
        if (PhotonNetwork.IsMasterClient)
        {
            MonitorManager.DealDamageToMonitor("tutorial");
        }
    }

    //炎に呼んでもらう
    public void CompleteCooled()
    {
        StartCoroutine(CoCompleteCooled());
    }
    IEnumerator CoCompleteCooled()
    {
        allOKObject.SetActive(true);

        yield return new WaitForSeconds(3);

        GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
    }
}
