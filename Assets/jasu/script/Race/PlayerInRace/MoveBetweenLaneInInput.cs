using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenLaneInInput : MoveBetweenLane
{
    [SerializeField]
    float padInputRangeY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        laneNum = laneManager.GetLaneNum() - 1;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = 0;

        // 接地中
        if (colliderSensorFront.GetExistInCollider() ||
            colliderSensorBack.GetExistInCollider())
        {
            if (TetraInput.sTetraPad.GetVector().y > padInputRangeY)
            {
                moveDir = -1f;
                arrivalLane = false;

                if (transform.position.x <= laneManager.GetLanePosX(belongingLaneId))
                {
                    laneIdUpdated = false;
                }

                if (!laneIdUpdated)
                {
                    laneIdUpdated = true;

                    belongingLaneId++;
                    if (belongingLaneId > laneNum)
                    {
                        belongingLaneId = laneNum;
                    }
                }
            }
            else if (TetraInput.sTetraPad.GetVector().y < -padInputRangeY)
            {
                moveDir = 1f;
                arrivalLane = false;

                if (transform.position.x >= laneManager.GetLanePosX(belongingLaneId))
                {
                    laneIdUpdated = false;
                }

                if (!laneIdUpdated)
                {
                    laneIdUpdated = true;

                    belongingLaneId--;
                    if (belongingLaneId < 0)
                    {
                        belongingLaneId = 0;
                    }
                }
            }
        }

        // レーンの中心を保つ
        if (arrivalLane)
        {
            float LaneX = laneManager.GetLanePosX(belongingLaneId);
            Vector3 pos = transform.position;
            pos.x = LaneX;
            transform.position = pos;
        }
    }

    private void FixedUpdate()
    {
        if (moveDir != 0)    // 入力時移動
        {
            rb.velocity = new Vector3(moveDir * spd, rb.velocity.y, rb.velocity.z);

            // 移動制限
            if (belongingLaneId >= laneNum &&
                transform.position.x < laneManager.GetLanePosX(laneNum) - outsideRange)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }

            if (belongingLaneId <= 0 &&
                transform.position.x > laneManager.GetLanePosX(0) + outsideRange)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (!arrivalLane)   // 所属レーンへの補正
        {
            float LaneX = laneManager.GetLanePosX(belongingLaneId);
            float DirX = LaneX - transform.position.x;
            if (Mathf.Abs(DirX) < 1f)
            {
                arrivalLane = true;
            }
            else
            {
                if (DirX > 0)
                {
                    rb.velocity = new Vector3(spd, rb.velocity.y, rb.velocity.z);
                }
                else
                {
                    rb.velocity = new Vector3(-spd, rb.velocity.y, rb.velocity.z);
                }
            }
        }
    }
}
