using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoolingManager : MonoBehaviour
{
    [SerializeField] Trisibo.SceneField nextScene;

    // Start is called before the first frame update
    void Start()
    {
        //Tutorial用の炎を出す

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //炎に呼んでもらう
    public void CompleteCooled()
    {
        StartCoroutine(CoCompleteCooled());
    }

    IEnumerator CoCompleteCooled()
    {

        yield return null;

        GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
    }
}
