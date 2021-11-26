using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour
{
    [Header("Require Reference")]
    [SerializeField] CassetteManager cassetteManager;

    [Header("Scene")]
    [SerializeField] SceneObject titleScene;
    [SerializeField] SceneObject resultScene;

    private void Start()
    {
        GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(titleScene);
    }

    // Update is called once per frame
    void Update()
    {
        //終了チェック(必ず存在確認はする)
        if (GameInGameManager.sCurrentGameInGameManager && GameInGameManager.sCurrentGameInGameManager.isGameEnd)
        {
            Debug.Log(GameInGameManager.sCurrentGameInGameManager.gameName + "をクリアしました！！！！");

            //カセット吐き出したり
            cassetteManager.ActiveCassetIsClearOn();

            //タイマー計測したり

            //すべてクリアしたら結果画面へ
            if (cassetteManager.CheckAllCassette())
            {
                //Debug.LogWarning("強制全クリ発動！！");
                GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(resultScene);
            }
            else
            { //ゲームを落とす
                GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene("");

                //カセット再表示
                cassetteManager.AppearAllCassette();
            }
        }
    }


}
