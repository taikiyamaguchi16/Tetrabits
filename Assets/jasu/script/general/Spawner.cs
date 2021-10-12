using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public enum HowToSpawn
{
    [Tooltip("スタート時のみに生成")]
    Start = 0,
    [Tooltip("設定された間隔で生成")]
    Interval,
}

public class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("スポーンされるプレハブ")]
    GameObject prefabToSpawn;

    [SerializeField, Tooltip("生成位置")]
    Vector3 spawnPosition;

    [SerializeField, Tooltip("生成角度")]
    Vector3 spawnEuler;

    [SerializeField,Tooltip("生成方法")]
    HowToSpawn howToSpawn;

    [SerializeField, Tooltip("インターバル秒数")]
    float intervalSeconds = 1f;

    float timer = 0f;

    [SerializeField, Tooltip("スタート時に生成するか否か")]
    bool spawnWhenStart = true;

    [Tooltip("スポーン可能かどうか")]
    public bool spawnable = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへ参加しました");
        if (spawnWhenStart && spawnable)
            SpawnPrefab();
    }
    private void Update()
    {
        switch (howToSpawn)
        {
            case HowToSpawn.Interval:
                timer += Time.deltaTime;
                if(timer >= intervalSeconds)
                {
                    SpawnPrefab();
                    timer = 0;
                }
                break;
            default:
                break;
        }
    }

    public GameObject SpawnPrefab()
    {
        if (spawnable)
        {
            GameObject spawned = PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPosition,Quaternion.Euler(spawnEuler));
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Vector3 _pos, Vector3 _euler)
    {
        if (spawnable)
        {
            GameObject spawned = Instantiate(prefabToSpawn);
            spawned.transform.position = _pos;
            spawned.transform.rotation = Quaternion.Euler(_euler);
            return spawned;
        }
        return null;
    }
}
