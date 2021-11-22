using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEnemyGenerator : MonoBehaviour
{
    [Header("Reference Waypoints")]
    [SerializeField] List<Transform> waypoints;

    [Header("Status")]
    [SerializeField] float arrivalSeconds = 1.0f;
    [SerializeField] int generateNum = 1;
    int generateCounter;

    [SerializeField] float generateIntervalSeconds = 1.0f;
    float generateIntervalTimer;

    [Header("Prefab Reference")]
    [SerializeField] GameObject chainEnemyPrefab;

    [Header("Debug")]
    [SerializeField] bool hideWaypoints = true;

    [ContextMenu("Generate")]
    public void Generate()
    {
        generateCounter = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        //生成されたことにしておく
        generateCounter = generateNum;

        if (hideWaypoints)
        {
            //waypointは位置さえ取れればいいのでアクティブを消しておく
            foreach (var point in waypoints)
            {
                point.gameObject.SetActive(false);
            }

            //自身のレンダラーも消しておく
            var r = GetComponent<Renderer>();
            if (r)
            {
                r.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (generateCounter < generateNum)
        {
            generateIntervalTimer += Time.deltaTime;
            if (generateIntervalTimer >= generateIntervalSeconds)
            {
                generateIntervalTimer = 0.0f;
                generateCounter++;

                //生成
                var obj = Instantiate(chainEnemyPrefab);
                GameInGameUtil.MoveGameObjectToOwnerScene(obj, gameObject);
                var chainEnemy = obj.GetComponent<ChainEnemy>();
                chainEnemy.waypoints = waypoints;
                chainEnemy.arrivalSeconds = arrivalSeconds;
            }
        }
    }
}
