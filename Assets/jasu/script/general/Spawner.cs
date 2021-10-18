using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HowToSpawn
{
    [Tooltip("スタート時のみに生成")]
    Start = 0,
    [Tooltip("設定された間隔で生成")]
    Interval,
}

[System.Serializable]
public class SpawnInfo
{
    [Tooltip("スポーンされるプレハブ")]
    public GameObject prefabToSpawn;

    [Tooltip("生成Transform(manualオフ)")]
    public Transform spawnTrans = null;

    [Tooltip("生成位置(manualオン)")]
    public Vector3 spawnPosition;

    [Tooltip("生成角度(manualオン)")]
    public Vector3 spawnEuler;

    [Tooltip("自動生成の方法")]
    public HowToSpawn howToSpawn;

    [Tooltip("インターバル秒数")]
    public float intervalSeconds = 1f;

    [HideInInspector]
    public float timer = 0f;

    [Tooltip("trueなら入力した位置と向き、falseならアタッチされたTransformにスポーン、")]
    public bool manual = true;

    [Tooltip("スタート時に生成するか否か")]
    public bool spawnWhenStart = true;

    [Tooltip("スポーン可能かどうか")]
    public bool spawnable = true;

    public GameObject SpawnPrefab()
    {
        if (spawnable)
        {
            GameObject spawned = Object.Instantiate(prefabToSpawn);
            if (manual)
            {
                spawned.transform.position = spawnPosition;
                spawned.transform.rotation = Quaternion.Euler(spawnEuler);
            }
            else
            {
                spawned.transform.position = spawnTrans.position;
                spawned.transform.rotation = spawnTrans.rotation;
            }
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Vector3 _pos, Vector3 _euler)
    {
        if (spawnable)
        {
            GameObject spawned = Object.Instantiate(prefabToSpawn);
            spawned.transform.position = _pos;
            spawned.transform.rotation = Quaternion.Euler(_euler);
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Transform _spawnTrans)
    {
        if (spawnable)
        {
            GameObject spawned = Object.Instantiate(prefabToSpawn);
            spawned.transform.position = _spawnTrans.position;
            spawned.transform.rotation = _spawnTrans.rotation;
            return spawned;
        }
        return null;
    }
}

public class Spawner : MonoBehaviour
{
    [SerializeField,Tooltip("スポーン情報")]
    SpawnInfo spawnInfo;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnInfo.spawnWhenStart && spawnInfo.spawnable)
            spawnInfo.SpawnPrefab();
    }

    private void Update()
    {
        switch (spawnInfo.howToSpawn)
        {
            case HowToSpawn.Interval:
                spawnInfo.timer += Time.deltaTime;
                if(spawnInfo.timer >= spawnInfo.intervalSeconds)
                {
                    spawnInfo.SpawnPrefab();
                    spawnInfo.timer = 0;
                }
                break;
            default:
                break;
        }
    }

    public SpawnInfo GetSpawnInfo()
    {
        return spawnInfo;
    }
}
