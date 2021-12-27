using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerLaneMove : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    Rigidbody rb = null;

    [SerializeField]
    LaneManager laneManager = null;

    public int belongingLaneId { get; set; } = 0;

    [SerializeField]
    float spd;

    int laneIndexMax = 0;

    public int GetLaneIndexMax() { return laneIndexMax; }

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

    [SerializeField]
    bool isInputting;

    // Start is called before the first frame update
    void Start()
    {
        rb = racerController.GetRigidbody();
        laneIndexMax = laneManager.GetLaneNum() - 1;
        padPos = TetraInput.sTetraPad.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputByAverage();

        if (isInputting)
        {
            if(transform.position.x > laneManager.GetLanePosX(0) + 2f)
            {
                Vector3 pos = transform.position;
                pos.x = laneManager.GetLanePosX(0) + 2f;
                transform.position = pos;
            }
            else if(transform.position.x < laneManager.GetLanePosX(laneIndexMax) - 2f)
            {
                Vector3 pos = transform.position;
                pos.x = laneManager.GetLanePosX(laneIndexMax) - 2f;
                transform.position = pos;
            }
        }
        // レーンの中心を保つ
        else if (arrivalLane)
        {
            float LaneX = laneManager.GetLanePosX(belongingLaneId);
            Vector3 pos = transform.position;
            pos.x = LaneX;
            transform.position = pos;
        }
        else if (!isInputting && !arrivalLane)
        {
            int nearIndex = 0;
            for (int i = 1; i < laneManager.GetLaneNum(); i++)
            {
                if (Mathf.Abs(laneManager.GetLanePosX(i) - transform.position.x) < Mathf.Abs(laneManager.GetLanePosX(nearIndex) - transform.position.x))
                {
                    nearIndex = i;
                }
            }

            SetMoveLane(nearIndex);
        }
    }

    private void FixedUpdate()
    {
        if (isInputting)
        {
            if (inputY < 0)
            {
                rb.velocity = new Vector3(spd, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                rb.velocity = new Vector3(-spd, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (!arrivalLane)   // 所属レーンへの補正
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
        else if (_laneId > laneIndexMax)
        {
            _laneId = laneIndexMax;
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
        isInputting = false;
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

                if(Mathf.Abs(inputY) > padInputRangeY)
                {
                    isInputting = true;
                    arrivalLane = false;
                }
                //inputY = TetraInput.sTetraPad.GetVector().y / TetraInput.sTetraPad.GetNumOnPad();
            }

            // デバッグ
            if (Input.GetKey(KeyCode.W))
            {
                inputY = 1f;
                isInputting = true;
                arrivalLane = false;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                inputY = -1f;
                isInputting = true;
                arrivalLane = false;
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
