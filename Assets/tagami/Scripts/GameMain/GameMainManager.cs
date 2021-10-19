using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //終了チェック(必ず存在確認はする)
        if (GameInGameManager.sCurrentGameInGameManager && GameInGameManager.sCurrentGameInGameManager.isGameEnd)
        {
            Debug.Log(GameInGameManager.sCurrentGameInGameManager.gameName + "をクリアしました！！！！");

            //ゲームを落とす
            GetComponent<GameInGameSwitcher>().SwitchGameInGameScene("");

            //カセット吐き出したり

            //タイマー計測したり

            //すべてクリアしたら結果画面へ
        }
    }


}
