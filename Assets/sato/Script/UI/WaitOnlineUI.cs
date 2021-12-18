using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitOnlineUI : MonoBehaviourPunCallbacks
{
    private const int _PLAYER_UPPER_LIMIT = 4;

    [SerializeField]
    [Header("プレイヤー入れる")]
    GameObject[] Players;

    [SerializeField]
    [Header("Spawner設定")]
    GameObject[] Spawners;

    List<bool> isPlayerInstantiate = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneUnloaded += SceneUnloaded;

        // フラグ初期化
        for (int i = 0; i < _PLAYER_UPPER_LIMIT; i++)
        {
            isPlayerInstantiate.Add(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        WaitPlayerInstantiate();
    }

    void WaitPlayerInstantiate()
    {
        if (isPlayerInstantiate[PhotonNetwork.LocalPlayer.GetPlayerNum()])
        {
            PhotonNetwork.Instantiate("WaitOnline/" + Players[PhotonNetwork.LocalPlayer.GetPlayerNum()].name,
                Spawners[PhotonNetwork.LocalPlayer.GetPlayerNum()].transform.position,
                Quaternion.identity);
            isPlayerInstantiate[PhotonNetwork.LocalPlayer.GetPlayerNum()] = false;
        }
    }

    // シーンアンロードで生成したプレファブを消去
    void SceneUnloaded(Scene thisScene)
    {
        Destroy(GameObject.Find("Keirei_Yellow(Clone)"));
        Destroy(GameObject.Find("Keirei_Red(Clone)"));
        Destroy(GameObject.Find("Keirei_Green(Clone)"));
        Destroy(GameObject.Find("Keirei_Blue(Clone)"));
    }
}
