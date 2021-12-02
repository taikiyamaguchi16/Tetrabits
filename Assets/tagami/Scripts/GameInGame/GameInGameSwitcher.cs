using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class GameInGameSwitcher : MonoBehaviourPunCallbacks
{
    [Header("Noise")]
    [SerializeField] CRTNoise crtNoise;
    [SerializeField] float noiseDuration = 1.0f;

    [Header("Switch Scene On Debug Window")]
    [SerializeField] List<Trisibo.SceneField> gameInGameScenesOnDebugWindow;

    //現在のゲーム内ゲーム名
    string currentGameInGameSceneName = "";

    string textField = "";

    public void OnGUIWindow()
    {
        //GUIStyle style = new GUIStyle();
        //style.fontSize = 25;
        //GUILayout.Label(nameof(GameInGameSwitcher), style);

        GUILayout.Label("====================");

        textField = GUILayout.TextField(textField);
        if (GUILayout.Button("上記のScene名に遷移します"))
        {
            GameInGameUtil.SwitchGameInGameScene(textField);
        }

        foreach (var sceneField in gameInGameScenesOnDebugWindow)
        {
            var sceneName = GameInGameUtil.GetSceneNameByBuildIndex(sceneField.BuildIndex);

            if (GUILayout.Button(sceneName))
            {
                GameInGameUtil.SwitchGameInGameScene(sceneName);
            }
        }
    }

    public void CallSwitchGameInGameScene(string _nextSceneName)
    {
        photonView.RPC(nameof(RPCSwitchGameInGameScene), RpcTarget.All, _nextSceneName);
    }
    [PunRPC]
    public void RPCSwitchGameInGameScene(string _nextSceneName)
    {
        //現在のシーンを削除
        if (currentGameInGameSceneName.Length > 0)
        {
            Debug.Log(currentGameInGameSceneName + "：シーンを削除します");
            SceneManager.UnloadSceneAsync(currentGameInGameSceneName);
        }
        //次のシーンへ移行
        if (_nextSceneName.Length > 0)
        {
            Debug.Log(_nextSceneName + "：シーンへ移行します");
            SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("現在のシーン：" + currentGameInGameSceneName + "を削除しましたが、次のScene名が設定されていないので、移行できません");
        }

        //シーン名更新
        currentGameInGameSceneName = _nextSceneName;

        //ノイズをはしらせる
        if (crtNoise)
        {
            crtNoise.AlWaysNoiseWithTimeLimit(noiseDuration, false);
        }
    }
}
