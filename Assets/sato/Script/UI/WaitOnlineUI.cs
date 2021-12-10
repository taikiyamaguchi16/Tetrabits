using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class WaitOnlineUI : MonoBehaviour
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
        if (PhotonNetwork.InRoom)
        {
            switch (PhotonNetwork.LocalPlayer.GetPlayerNum())
            {
                case 0:
                    if (isPlayerInstantiate[0])
                    {
                        PhotonNetwork.Instantiate("WaitOnline/" + Players[0].name, Spawners[0].transform.position, Quaternion.identity);
                        isPlayerInstantiate[0] = false;
                    }
                    break;

                case 1:
                    if (isPlayerInstantiate[1])
                    {
                        PhotonNetwork.Instantiate("WaitOnline/" + Players[1].name, Spawners[1].transform.position, Quaternion.identity);
                        isPlayerInstantiate[1] = false;
                    }
                    break;

                case 2:
                    if (isPlayerInstantiate[2])
                    {
                        PhotonNetwork.Instantiate("WaitOnline/" + Players[2].name, Spawners[2].transform.position, Quaternion.identity);
                        isPlayerInstantiate[2] = false;
                    }
                    break;

                case 3:
                    if (isPlayerInstantiate[3])
                    {
                        PhotonNetwork.Instantiate("WaitOnline/" + Players[3].name, Spawners[3].transform.position, Quaternion.identity);
                        isPlayerInstantiate[3] = false;
                    }
                    break;
            }
        }
    }

    void SceneUnloaded(Scene thisScene)
    {
        Destroy(GameObject.Find("Keirei_Yellow(Clone)"));
        Destroy(GameObject.Find("Keirei_Red(Clone)"));
        Destroy(GameObject.Find("Keirei_Green(Clone)"));
        Destroy(GameObject.Find("Keirei_Blue(Clone)"));
    }
}
