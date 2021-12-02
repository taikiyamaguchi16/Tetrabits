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
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<GameInGameSwitcher>().CallSwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(waitInsertCassetteScene.BuildIndex));
            }
        }
    }


}
