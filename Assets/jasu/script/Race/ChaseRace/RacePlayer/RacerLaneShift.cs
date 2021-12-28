using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerLaneShift : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    Rigidbody rb = null;

    [SerializeField]
    LaneManager laneManager = null;

    public int belongingLaneId { get; set; } = 0;

    [SerializeField]
    float spd;

    int laneNum = 0;

    public int GetLaneNum() { return laneNum; }

    bool arrivalLane = false;

    public bool GetArrivalLane() { return arrivalLane; }

    [SerializeField]
    float padInputRangeY = 0.35f;

    Vector3 padPos;

    [SerializeField]
    float padLength = 5f;

    public bool movable { get; set; } = true;

    [Header("デバッグ用")]

    [SerializeField]
    bool contactSlope = false;

    [SerializeField]
    List<GameObject> contactSlopeList = new List<GameObject>();

    [SerializeField]
    float inputY;

    // Start is called before the first frame update
    void Start()
    {
        rb = racerController.GetRigidbody();
        laneNum = laneManager.GetLaneNum() - 1;
        padPos = TetraInput.sTetraPad.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputByAverage();

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
            // 到着判定
            float laneX = laneManager.GetLanePosX(belongingLaneId);
            float distanceX = laneX - transform.position.x;
            if (Mathf.Abs(distanceX) < 1f)
            {
                arrivalLane = true;
            }
            else
            {
                if (contactSlope)
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
                }
                else
                {
                    if (distanceX > 0)
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
        if (_laneId < 0)
        {
            _laneId = 0;
        }
        else if (_laneId > laneNum)
        {
            _laneId = laneNum;
        }

        // 接地中
        //if (racerController.GetRacerGroundSensor().GetOnGround())
        //{
        //    //if (!bikeSlipDown.isSliping)
        //    {

        //    }
        //}
        belongingLaneId = _laneId;
        arrivalLane = false;
    }

    private void InputByAverage()
    {
        inputY = 0f;
        if (movable)
        {
            if (TetraInput.sTetraPad.GetNumOnPad() > 0)
            {
                List<GameObject> onPadObjList = TetraInput.sTetraPad.GetObjectsOnPad();

                List<float> yLengthList = new List<float>();

                foreach (GameObject onPad in onPadObjList)
                {
                    yLengthList.Add(onPad.transform.position.z - padPos.z);
                }

                float total = 0;
                foreach (float yLength in yLengthList)
                {
                    total += yLength;
                }

                inputY = total / yLengthList.Count / padLength;

                //inputY = TetraInput.sTetraPad.GetVector().y / TetraInput.sTetraPad.GetNumOnPad();
            }

            // デバッグ
            //if (Input.GetKey(KeyCode.W))
            //{
            //    inputY = 1f;
            //}
            //else if (Input.GetKey(KeyCode.S))
            //{
            //    inputY = -1f;
            //}

            if (inputY < -padInputRangeY)
            {
                if (belongingLaneId != 0)
                {
                    SetMoveLane(0);
                }
            }
            else if (inputY > padInputRangeY)
            {
                if (belongingLaneId != 2)
                {
                    SetMoveLane(2);
                }
            }
            else
            {
                if (belongingLaneId != 1)
                {
                    SetMoveLane(1);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SlopeRoadInRace")
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
            if (contactSlopeList.Count <= 0)
            {
                contactSlopeList.Clear();
                contactSlope = false;
            }
        }
    }
}
