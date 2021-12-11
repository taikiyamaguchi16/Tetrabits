using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInGameResultManager : MonoBehaviour
{

    [Header("Result")]
    [SerializeField] List<GameInGameResultController> resultObjects;
    [SerializeField] Transform initialResultObjectPosition;
    [SerializeField] Vector3 resultObjectOffset;

    [Header("Next Scene")]
    [SerializeField] SceneObject nextScene;

    [Header("Debug")]
    [SerializeField] bool useDebugResult;

    private void Start()
    {       

        List<GameMainManager.GameInGameTimer> gameInGameTimers = new List<GameMainManager.GameInGameTimer>();
        if (useDebugResult)
        {//Debug用データ用意
            Debug.LogWarning("デバッグ用データでResultを出力します");
            {
                GameMainManager.GameInGameTimer timer = new GameMainManager.GameInGameTimer();
                timer.tag = "jump";
                timer.startTime = 30.0f;
                timer.endTime = 300.0f;
                gameInGameTimers.Add(timer);
            }
            {
                GameMainManager.GameInGameTimer timer = new GameMainManager.GameInGameTimer();
                timer.tag = "shooting";
                timer.startTime = 660.0f;
                timer.endTime = 950.0f;
                gameInGameTimers.Add(timer);
            }
            {
                GameMainManager.GameInGameTimer timer = new GameMainManager.GameInGameTimer();
                timer.tag = "race";
                timer.startTime = 460.0f;
                timer.endTime = 950.0f;
                gameInGameTimers.Add(timer);
            }
            {
                GameMainManager.GameInGameTimer timer = new GameMainManager.GameInGameTimer();
                timer.tag = "gyro";
                timer.startTime = 430.0f;
                timer.endTime = 933.0f;
                gameInGameTimers.Add(timer);
            }
        }
        else
        {
            var managerObj = GameObject.Find("GameMainManager");
            if (!managerObj)
            {
                Debug.LogError("GameMainManagerが見つかりませんでした");
                return;
            }
            gameInGameTimers = managerObj.GetComponent<GameMainManager>().GetGameInGameTimerList();
        }

        Vector3 resultObjectPosition = initialResultObjectPosition.position;
        foreach (var gameInGameTimer in gameInGameTimers)
        {
            bool exists = false; ;
            foreach (var resultObj in resultObjects)
            {
                if (resultObj.CompareGameInGameTag(gameInGameTimer.tag))
                {
                    //順番に並べていく
                    resultObj.SetPosition(resultObjectPosition);
                    resultObj.SetTime(gameInGameTimer.startTime, gameInGameTimer.endTime);
                    resultObjectPosition += resultObjectOffset;
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Debug.LogError(gameInGameTimer.tag + ":タグのゲームリザルト名は使用できません");
            }
        }
        //まだ並べられていないリザルトを並べる
        foreach (var resultObj in resultObjects)
        {
            if (!resultObj.IsSorted())
            {
                resultObj.SetPosition(resultObjectPosition);
                resultObjectPosition += resultObjectOffset;
            }
        }
    }//update

    // Update is called once per frame
    void Update()
    {
        //カメラよる
        VirtualCameraManager.OnlyActive(0);

        if ((XInputManager.GetButtonTrigger(0, XButtonType.A) || Input.GetKeyDown(KeyCode.Return)))
        {
            //コンテナをクリア
            NetworkObjContainer.NetworkObjDictionary.Clear();

            //ルーム解散処理？
            Photon.Pun.PhotonNetwork.LeaveRoom();
            Photon.Pun.PhotonNetwork.LeaveLobby();
            Photon.Pun.PhotonNetwork.Disconnect();


            //12/6 田上　初期化とか面倒すぎるのでコメントアウト
            //オフライン関数でタイトルへ戻る
            //GameInGameUtil.SwitchGameInGameSceneOffline(nextScene);


            //GameMainの再読み込み
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }
}
