using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerGroundSensor : MonoBehaviour
{
    [SerializeField]
    Transform rayTrans;

    [SerializeField]
    bool onGround;

    public bool GetOnGround() { return onGround; }

    [SerializeField]
    bool onDirt;

    public bool GetOnDirt() { return onDirt; }

    [SerializeField]
    bool onSlope;

    public bool GetOnSlope() { return onSlope; }

    [SerializeField]
    float rayLength = 2f;

    public Vector3 groundNormalVec { get; private set; }

    [SerializeField]
    List<GameObject> exceptionList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        onGround = false;
        onDirt = false;
        onSlope = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayPosition = rayTrans.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength))
        {
            onGround = true;
            foreach (var exception in exceptionList)
            {
                if (exception.GetInstanceID() == hitInfo.transform.gameObject.GetInstanceID())
                {
                    onGround = false;
                }
            }

            if(hitInfo.transform.tag == "Dirt")
            {
                onDirt = true;
            }
            else
            {
                onDirt = false;
            }

            if(hitInfo.transform.tag == "SlopeRoadInRace")
            {
                onSlope = true;
            }
            else
            {
                onSlope = false;
            }

            groundNormalVec = hitInfo.normal;
        }
        else
        {
            onGround = false;
            onDirt = false;
            onSlope = false;
        }
    }
}
