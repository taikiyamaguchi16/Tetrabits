using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject SpawnPrefab(int _num)
    {
        if (spawnList[_num].spawnable)
        {
            return spawnList[_num].SpawnPrefab();
        }
        return null;
    }

    public GameObject SpawnPrefab(int _num, Vector3 _pos, Vector3 _euler)
    {
        if (spawnList[_num].spawnable)
        {
            return spawnList[_num].SpawnPrefab(_pos, _euler);
        }
        return null;
    }

    public GameObject SpawnPrefab(int _num, Transform _trans)
    {
        if (spawnList[_num].spawnable)
        {
            return spawnList[_num].SpawnPrefab(_trans);
        }
        return null;
    }

    public List<GameObject> SpawnAllPrefab()
    {
        List<GameObject> spawnedList = new List<GameObject>();
        foreach(var spawn in spawnList)
        {
            spawnedList.Add(spawn.SpawnPrefab());
        }
        return null;
    }

    public List<SpawnInfo> GetSpawnInfoList()
    {
        return spawnList;
    }
}