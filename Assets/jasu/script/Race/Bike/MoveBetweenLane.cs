using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenLane : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rb = null;

    [SerializeField]
    protected LaneManager laneManager = null;

    public int belongingLaneId { get; set; } = 0;

    [SerializeField]
    protected BikeSlipDown bikeSlipDown;

    [SerializeField]
    protected ColliderSensor colliderSensorFront = null;

    [SerializeField]
    protected ColliderSensor colliderSensorBack = null;

    [SerializeField]
    protected float outsideRange = 3f;

    [SerializeField]
    protected float spd;

    protected float moveDir;

    protected int laneNum = 0;

    protected bool laneIdUpdated = false;

    protected bool arrivalLane = false;

    public int dirMove { get; set; } = 0;


    // Start is called before the first frame update
    void Start()
    {
        laneNum = laneManager.GetLaneNum() - 1;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (!arrivalLane)   // 所属レーンへの補正
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

    public void SetMoveLane(int _laneId)
    {
        // 接地中
        if (colliderSensorFront.GetExistInCollider() ||
            colliderSensorBack.GetExistInCollider())
        {
            if (!bikeSlipDown.isSliping)
            {
                belongingLaneId = _laneId;
                arrivalLane = false;
            }
        }
    }
}
