using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEnemy : MonoBehaviour
{
    [System.NonSerialized] public List<Transform> waypoints;
    [System.NonSerialized] public float arrivalSeconds = 3.0f;

    float totalStraightLineDistance;

    int sectionIndex;
    float sectionTimer;
    float sectionArrivalSeconds;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Count <= 1)
        {
            Debug.LogError("Waypointは２つ以上設定してください");
            return;
        }

        //全体距離を測る
        totalStraightLineDistance = 0.0f;
        for (int i = 0; i < (waypoints.Count - 1); i++)
        {
            totalStraightLineDistance += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
        }

        //最初の区間の到達時間を計算
        var sectionDistance = Vector3.Distance(waypoints[sectionIndex].position, waypoints[sectionIndex + 1].position);
        sectionArrivalSeconds = arrivalSeconds * sectionDistance / totalStraightLineDistance;
        //初期座標設定
        transform.position = waypoints[sectionIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        sectionTimer += Time.deltaTime;
        if (sectionTimer >= sectionArrivalSeconds)
        {
            if (sectionIndex < waypoints.Count - 2)
            {//次の区間が存在
                sectionIndex++;              
                //区間の距離を計測
                var sectionDistance = Vector3.Distance(waypoints[sectionIndex].position, waypoints[sectionIndex + 1].position);
                sectionArrivalSeconds = arrivalSeconds * sectionDistance / totalStraightLineDistance;
                sectionTimer = 0.0f;
            }
            else
            {
                //終了
                Destroy(gameObject);
            }
        }

        transform.position = Vector3.Lerp(waypoints[sectionIndex].position, waypoints[sectionIndex + 1].position, sectionTimer / sectionArrivalSeconds);
    }
}
