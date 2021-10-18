using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner_PunCollBack : MonoBehaviourPunCallbacks
{
    [SerializeField]
    SpawnInfo spawnInfo;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへ参加しました");
        if (spawnInfo.spawnWhenStart && spawnInfo.spawnable)
            SpawnPrefabPhoton();
    }
    private void Update()
    {
        switch (spawnInfo.howToSpawn)
        {
            case HowToSpawn.Interval:
                spawnInfo.timer += Time.deltaTime;
                if(spawnInfo.timer >= spawnInfo.intervalSeconds)
                {
                    SpawnPrefabPhoton();
                    spawnInfo.timer = 0;
                }
                break;
            default:
                break;
        }
    }

    public GameObject SpawnPrefabPhoton()
    {
        if (spawnInfo.spawnable)
        {
            if (spawnInfo.manual)
            {
                GameObject spawned = PhotonNetwork.Instantiate(spawnInfo.prefabToSpawn.name, spawnInfo.spawnPosition, Quaternion.Euler(spawnInfo.spawnEuler));
                return spawned;
            }
            else
            {
                GameObject spawned = PhotonNetwork.Instantiate(spawnInfo.prefabToSpawn.name, spawnInfo.spawnTrans.position, spawnInfo.spawnTrans.rotation);
                return spawned;
            }
        }
        return null;
    }

    public GameObject SpawnPrefab(Vector3 _pos, Vector3 _euler)
    {
        if (spawnInfo.spawnable)
        {
            GameObject spawned = PhotonNetwork.Instantiate(spawnInfo.prefabToSpawn.name, _pos, Quaternion.Euler(_euler));
            return spawned;
        }
        return null;
    }

    public GameObject SpawnPrefab(Transform _spawnTrans)
    {
        if (spawnInfo.spawnable)
        {
            GameObject spawned = PhotonNetwork.Instantiate(spawnInfo.prefabToSpawn.name, _spawnTrans.position, _spawnTrans.rotation);
            return spawned;
        }
        return null;
    }
}
