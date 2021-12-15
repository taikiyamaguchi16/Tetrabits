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
    protected ColliderSensor colliderSensor = null;

    [SerializeField]
    protected float spd;

    protected float moveDir;

    protected int laneNum = 0;

    public int GetLaneNum() { return laneNum; }

    protected bool laneIdUpdated = false;

    protected bool arrivalLane = false;

    public bool GetArrivalLane() { return arrivalLane; } 

    public int dirMove { get; set; } = 0;

    [SerializeField]
    bool contactSlope = false;

    List<GameObject> contactSlopeList = new List<GameObject>();

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
                //Vector3 rayPosition = transform.position;
                //rayPosition.y -= 0.8f;
                //Ray ray = new Ray(rayPosition, Vector3.right);
                //if (Physics.Raycast(ray, out RaycastHit rightHitInfo, 1f))
                //{
                //    if (rightHitInfo.transform.tag == "FlatRoadInRace" ||
                //        rightHitInfo.transform.tag == "SlopeRoadInRace")
                //        contactSlope = true;
                //}
                //else
                //{
                //    ray = new Ray(rayPosition, Vector3.left);
                //    if (Physics.Raycast(ray, out RaycastHit leftHitInfo, 1f))
                //    {
                //        if (leftHitInfo.transform.tag == "FlatRoadInRace" ||
                //                leftHitInfo.transform.tag == "SlopeRoadInRace")
                //            contactSlope = true;
                //    }
                //    else
                //    {
                //        contactSlope = false;
                //    }
                //}

                //velo = rb.velocity;

                if (contactSlope)
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
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

    public void SetMoveLane(int _laneId)
    {
        if(_laneId < 0)
        {
            _laneId = 0;
        }
        else if(_laneId > laneNum)
        {
            _laneId = laneNum;
        }

        // 接地中
        if (colliderSensor.GetExistInCollider())
        {
            if (!bikeSlipDown.isSliping)
            {
                belongingLaneId = _laneId;
                arrivalLane = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SlopeRoadInRace")
        {
            contactSlopeList.Add(collision.gameObject);
            contactSlope = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "SlopeRoadInRace")
        {
            contactSlopeList.Remove(collision.gameObject);
            if(contactSlopeList.Count <= 0)
            {
                contactSlopeList.Clear();
                contactSlope = false;
            }
        }
    }
}
