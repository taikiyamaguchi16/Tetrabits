using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBatteryManager : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] List<TutorialBatteryLamp> batteryLamps;

    [Header("Next")]
    [SerializeField] Trisibo.SceneField nextScene;
    [SerializeField] GameObject allOKObject;
    bool switched;

    [Header("UI")]
    [SerializeField] Text numBatteryOnText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int numBatteryExists = 0;
        foreach (var lamp in batteryLamps)
        {
            if (lamp.batteryHolder && lamp.batteryHolder.GetBatterylevel() > 0)
            {
                numBatteryExists++;
            }
        }

        //UIの更新
        numBatteryOnText.text = "残り" + numBatteryExists + "/" + batteryLamps.Count;

        if (!switched && numBatteryExists >= batteryLamps.Count)
        {
            switched = true;
            //ALL OK!
            StartCoroutine(SwitchNextScene());
        }

    }

    IEnumerator SwitchNextScene()
    {
        allOKObject.SetActive(true);

        yield return new WaitForSeconds(3);

        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
        }
    }
}
