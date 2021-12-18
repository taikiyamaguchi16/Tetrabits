using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMainManager : MonoBehaviourPunCallbacks
{
    [Header("Require Reference")]
    [SerializeField] CassetteManager cassetteManager;

    [Header("Scene")]
    [SerializeField] SceneObject titleScene;
    [SerializeField] SceneObject resultScene;
    [SerializeField] Trisibo.SceneField waitInsertCassetteScene;

    [Header("Sound")]
    [SerializeField] AudioClip waitInsertCassetteBGMClip;
    [SerializeField] float waitInsertCassetteBGMVolumeScale = 1.0f;

    public class GameInGameTimer
    {
        public string tag;
        public float startTime;
        public float endTime;
    }
    List<GameInGameTimer> gameInGameTimerList = new List<GameInGameTimer>();

    private void Start()
    {
        GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(titleScene);
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー計測する？


        //多分ここのGameEndの通知マスタークライアントに処理させたほうがいい
        //終了チェック(必ず存在確認はする)
        if (PhotonNetwork.IsMasterClient && GameInGameManager.sCurrentGameInGameManager && GameInGameManager.sCurrentGameInGameManager.IsGameEndTrigger())
        {
            CallClearCurrentGameInGame(GameInGameManager.sCurrentGameInGameManager.gameName, 0.0f);
        }
    }
    void CallClearCurrentGameInGame(string _gameInGameName, float _clearSeconds)
    {
        photonView.RPC(nameof(RPCClearCurrentGameInGame), RpcTarget.AllViaServer, _gameInGameName, _clearSeconds);
    }
    [PunRPC]
    public void RPCClearCurrentGameInGame(string _gameInGameName, float _clearSeconds)
    {
        Debug.Log(_gameInGameName + "を" + _clearSeconds + "秒でクリア");

        //カセット吐き出し
        cassetteManager.ActiveCassetIsClearOn();

        //すべてクリアしたら結果画面へ
        if (cassetteManager.CheckAllCassette())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(resultScene);
            }
        }
        else
        { //カセット待機シーンへ
            //BGM戻しておく
            SimpleAudioManager.PlayBGMCrossFade(waitInsertCassetteBGMClip, 1.0f, waitInsertCassetteBGMVolumeScale);

            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(waitInsertCassetteScene.BuildIndex));
            }
        }
    }

    //**********************************************************
    //GameInGameに呼んでもらう
    public void StartGameInGameTimer(string _gameTag)
    {
        foreach (var timer in gameInGameTimerList)
        {
            if (timer.tag == _gameTag)
            {
                Debug.Log("そのゲームタグはすでに登録されているのでタイマーを作成しません");
                return;
            }
        }

        //タイマー作成
        GameInGameTimer gameInGameTimer = new GameInGameTimer();
        gameInGameTimer.tag = _gameTag;
        gameInGameTimer.startTime = Time.realtimeSinceStartup;
        gameInGameTimerList.Add(gameInGameTimer);
    }

    public void StopGameInGameTimer(string _gameTag)
    {
        //タイマーを取得
        GameInGameTimer gameInGameTimer = null;
        foreach (var timer in gameInGameTimerList)
        {
            if (timer.tag == _gameTag)
            {
                gameInGameTimer = timer;
                break;
            }
        }
        if (gameInGameTimer == null)
        {
            Debug.LogError(_gameTag + ":タグのタイマーが作成されていません　StartGameInGameTimer(string)を呼んでください");
            return;
        }

        gameInGameTimer.endTime = Time.realtimeSinceStartup;
    }

    //**********************************************************
    //Resultで使用
    public List<GameInGameTimer> GetGameInGameTimerList()
    {
        return gameInGameTimerList;
    }

}
