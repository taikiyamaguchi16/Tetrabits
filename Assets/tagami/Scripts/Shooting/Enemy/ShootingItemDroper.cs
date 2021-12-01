using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingItemDroper : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] GameObject bombItemPrefab;
    [SerializeField, Range(0, 100)] int bombDropPercent;
    [Header("LevelUp")]
    [SerializeField] GameObject levelUpItemPrefab;
    [SerializeField, Range(0, 100)] int levelUpDropPercent;

    private static bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            bool created = false;

            if (!created && Random.Range(0, 100) < bombDropPercent)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    ShootingGameManager.sShootingGameManager?.CallLocalInstantiate(bombItemPrefab.name, transform.position, Quaternion.identity);
                }              
                created = true;
            }
            //12/1 現在の仕様ではレベルアップアイテムを使用しない
            //if (!created && Random.Range(0, 100) < levelUpDropPercent)
            //{
            //    if (PhotonNetwork.IsMasterClient)
            //    {
            //        ShootingGameManager.sShootingGameManager.CallLocalInstantiate(levelUpItemPrefab.name, transform.position, Quaternion.identity);
            //    }
            //    created = true;
            //}
        }
    }
}
