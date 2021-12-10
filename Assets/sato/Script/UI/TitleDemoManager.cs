using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDemoManager : MonoBehaviour
{
    //[SerializeField]
    //[Header("横方向に動くプレイヤーの上限数")]
    //int playerUpperLimitX = 2;

    //[SerializeField]
    //[Header("前方向に動くプレイヤーの上限数")]
    //int playerUpperLimitZ = 2;

    [SerializeField]
    [Header("プレイヤー上限数")]
    int playerUpperLimit = 1;

    [SerializeField]
    [Header("横方向プレイヤー入れる")]
    GameObject[] PlayersX;

    [SerializeField]
    [Header("前方向プレイヤー入れる")]
    GameObject[] PlayersZ;

    [SerializeField]
    [Header("左方向Spawner設定")]
    GameObject SpawnerLeft;

    [SerializeField]
    [Header("右方向Spawner設定")]
    GameObject SpawnerRight;

    [SerializeField]
    [Header("前方向Spawner設定")]
    GameObject[] SpawnerZ;

    // ランダム格納用
    private int randomNum;
    private int randomPos;

    // 生成フラグ
    private bool isInstantiate = false;

    // 人数カウント用
    //private int playerCountX = 0;
    //private int playerCountZ = 0;
    private int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInstantiate();
    }

    //--------------------------------------------------
    // PlayerInstantiate
    // プレイヤー生成
    //--------------------------------------------------
    void PlayerInstantiate()
    {
        // 生成位置指定用
        randomPos = Random.Range(0, 2);

        switch (randomPos)
        {
            case 0:
                // X方向生成
                if (isInstantiate && playerCount < playerUpperLimit)
                {
                    // 生成位置指定用
                    randomPos = Random.Range(0, 2);

                    // 生成プレイヤーランダム
                    randomNum = Random.Range(0, PlayersX.Length);

                    switch (randomPos)
                    {
                        // 左
                        case 0:
                            Instantiate(PlayersX[randomNum], SpawnerLeft.transform.position, Quaternion.identity);
                            playerCount++;
                            break;

                        // 右
                        case 1:
                            Instantiate(PlayersX[randomNum], SpawnerRight.transform.position, Quaternion.identity);
                            playerCount++;
                            break;

                        default:
                            break;
                    }
                }
                break;

            case 1:
                // Z方向生成
                if (isInstantiate && playerCount < playerUpperLimit)
                {
                    // 生成位置指定用
                    randomPos = Random.Range(0, SpawnerZ.Length);

                    // 生成プレイヤーランダム
                    randomNum = Random.Range(0, PlayersZ.Length);

                    // 生成
                    Instantiate(PlayersZ[randomNum], SpawnerZ[randomPos].transform.position, Quaternion.identity);
                    playerCount++;
                }
                break;

            default:
                break;
        }
        

        

        // 生成制限
        if(playerCount > playerUpperLimit)
        {
            isInstantiate = false;
        }
        else
        {
            isInstantiate = true;
        }

        //if(playerCountX > playerUpperLimitX && playerCountZ > playerUpperLimitZ)
        //{
        //    isInstantiate = false;
        //}
        //else
        //{
        //    isInstantiate = true;
        //}
    }

    //--------------------------------------------------
    // PlayerCountDown
    // プレイヤーカウントダウン
    //--------------------------------------------------
    public void PlayerCountDown()
    {
        playerCount--;
    }

    //--------------------------------------------------
    // PlayerCountDownX
    // プレイヤーXカウントダウン
    //--------------------------------------------------
    //public void PlayerCountDownX()
    //{
    //    playerCountX--;
    //}

    //--------------------------------------------------
    // PlayerCountDownZ
    // プレイヤーZカウントダウン
    //--------------------------------------------------
    //public void PlayerCountDownZ()
    //{
    //    playerCountZ--;
    //}
}
