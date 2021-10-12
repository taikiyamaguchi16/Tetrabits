using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    [Tooltip("スポーンされるプレハブ")]
    public GameObject prefabToSpawn;

    [Tooltip("生成位置")]
    public Vector3 spawnPosition;

    [Tooltip("生成角度")]
    public Vector3 spawnEuler;

    [Tooltip("生成方法")]
    public HowToSpawn howToSpawn;

    [Tooltip("インターバル秒数")]
    public float intervalSeconds = 1f;

    [HideInInspector]
    public float timer = 0f;

    [Tooltip("スタート時に生成するか否か")]
    public bool spawnWhenStart = true;

    [Tooltip("スポーン可能かどうか")]
    public bool spawnable = true;

    public void SpawnPrefab()
    {
        if (spawnable)
        {
            GameObject spawned = Object.Instantiate(prefabToSpawn);
            spawned.transform.position = spawnPosition;
            spawned.transform.rotation = Quaternion.Euler(spawnEuler);
        }
    }
}


public class MultipleSpawner : MonoBehaviour
{
    [SerializeField]
    List<SpawnInfo> spawnList = new List<SpawnInfo>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var spawn in spawnList)
        {
            if(spawn.spawnWhenStart && spawn.spawnable)
            {
                GameObject spawned = Instantiate(spawn.prefabToSpawn);
                spawned.transform.position = spawn.spawnPosition;
                spawned.transform.rotation = Quaternion.Euler(spawn.spawnEuler);
            }
        }
    }

    private void Update()
    {
        foreach (var spawn in spawnList)
        {
            switch (spawn.howToSpawn)
            {
                case HowToSpawn.Interval:
                    spawn.timer += Time.deltaTime;
                    if (spawn.timer >= spawn.intervalSeconds)
                    {
                        spawn.SpawnPrefab();
                        spawn.timer = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public GameObject SpawnPrefab()
    {
        if (spawnList[0].spawnable)
        {
            GameObject spawned = Instantiate(spawnList[0].prefabToSpawn);
            spawned.transform.position = spawnList[0].spawnPosition;
            spawned.transform.rotation = Quaternion.Euler(spawnList[0].spawnEuler);
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(int _num)
    {
        if (spawnList[_num].spawnable)
        {
            GameObject spawned = Instantiate(spawnList[_num].prefabToSpawn);
            spawned.transform.position = spawnList[_num].spawnPosition;
            spawned.transform.rotation = Quaternion.Euler(spawnList[_num].spawnEuler);
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Vector3 _pos, Vector3 _euler)
    {
        if (spawnList[0].spawnable)
        {
            GameObject spawned = Instantiate(spawnList[0].prefabToSpawn);
            spawned.transform.position = _pos;
            spawned.transform.rotation = Quaternion.Euler(_euler);
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Vector3 _pos, Vector3 _euler, int _num)
    {
        if (spawnList[_num].spawnable)
        {
            GameObject spawned = Instantiate(spawnList[_num].prefabToSpawn);
            spawned.transform.position = _pos;
            spawned.transform.rotation = Quaternion.Euler(_euler);
            return spawned;
        }
        return null;
    }
}