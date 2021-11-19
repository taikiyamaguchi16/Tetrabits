using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    [SerializeField]
    Transform checkPoint = null;

    [SerializeField, Tooltip("通過判定の計算をしだすチェックポイントとの距離")]
    float passJudgeRange = 100f;

    [SerializeField]
    RaceManager raceManager;

    [SerializeField]
    BikeCtrlWhenStartAndGoal bikeCtrlWhenStartAndGoal;

    public int actualLapCount { get; private set; } = 0;

    public int lapCount { get; private set; } = 0;

    Vector3 dirOfLane;  // 進行方向

    Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        dirOfLane = checkPoint.forward;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(checkPoint.position,transform.position) < passJudgeRange &&
            Vector3.Distance(transform.position, oldPos) < passJudgeRange) // 範囲を超える瞬間移動は通過とみなさない
        {
            // 順方向に通過したとき
            if (Vector3.Dot(transform.position - checkPoint.position, dirOfLane) > 0 &&
                Vector3.Dot(oldPos - checkPoint.position, dirOfLane) <= 0)
            {
                // actualLapCountと同じならラップを加算する
                if (actualLapCount == lapCount) 
                {
                    lapCount++;
                    if(lapCount > raceManager.GetLapNum)    // ゴール判定
                    {
                        Debug.Log("ゴール");
                        bikeCtrlWhenStartAndGoal.SetActiveOffAfterGoal(false);
                        bikeCtrlWhenStartAndGoal.SetActiveOnAfterGoal(true);
                    }
                }
                actualLapCount++;
            }
            else if (Vector3.Dot(transform.position - checkPoint.position, dirOfLane) <= 0 &&
                Vector3.Dot(oldPos - checkPoint.position, dirOfLane) > 0) // 逆走したらactualLapCountをマイナス
            {
                actualLapCount--;
            }
        }

        oldPos = transform.position;
    }
}
