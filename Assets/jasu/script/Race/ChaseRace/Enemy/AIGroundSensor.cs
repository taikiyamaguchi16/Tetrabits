using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroundSensor : MonoBehaviour
{
    [SerializeField]
    bool onGround;

    public bool GetOnGround() { return onGround; }

    [SerializeField]
    float rayLength = 2f;

    [SerializeField]
    Transform rayTrans;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayPosition = rayTrans.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayLength))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
}
