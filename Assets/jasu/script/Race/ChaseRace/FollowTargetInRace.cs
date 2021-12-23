using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetInRace : MonoBehaviour
{
    [SerializeField]
    Transform followTrans;

    [SerializeField]
    RacerController racerController;

    // シーン上に配置したこのオブジェクトとfollowTransとの距離
    [SerializeField]
    Vector3 offset;

    [SerializeField]
    float offsetMinZ = -10f;

    [SerializeField]
    float offsetMaxZ = 20f;

    [SerializeField]
    Vector3 activeOffset;

    public Vector3 GetOffset() { return activeOffset; }

    [SerializeField]
    bool followX = true;

    [SerializeField]
    bool followY = true;

    [SerializeField]
    bool followZ = true;

    [SerializeField]
    bool self = false;

    float spdMin;

    float spdMax;

    // Start is called before the first frame update
    void Start()
    {
        float[] moveSpds = racerController.GetRacerMove().GetMoveSpdGears();
        spdMin = moveSpds[0];
        spdMax = moveSpds[moveSpds.Length - 1];

        activeOffset = offset;
        //offset = transform.localPosition - followTrans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float velocityZ = racerController.GetRigidbody().velocity.z;

        float rate = (velocityZ - spdMin) / (spdMax - spdMin);
        if (rate < 0f) rate = 0f;
        else if (rate > 1f) rate = 1f;

        Vector3 targetOffset = activeOffset;
        targetOffset.x = offset.x;
        targetOffset.y = offset.y;
        targetOffset.z = offsetMaxZ + (offsetMinZ - offsetMaxZ) * rate;

        activeOffset = Vector3.Lerp(activeOffset, targetOffset, Time.deltaTime);

        if (self)
        {
            Follow();
        }
    }

    public void Follow()
    {
        Vector3 pos = activeOffset;
        if (followX)
        {
            pos.x += followTrans.position.x;
        }

        if (followY)
        {
            pos.y += followTrans.position.y;
        }

        if (followZ)
        {
            pos.z += followTrans.position.z;
        }
        transform.position = pos;
    }

    public void Follow(Vector3 _offset)
    {
        Vector3 pos = _offset;
        if (followX)
        {
            pos.x += followTrans.position.x;
        }

        if (followY)
        {
            pos.y += followTrans.position.y;
        }

        if (followZ)
        {
            pos.z += followTrans.position.z;
        }
        transform.position = pos;
    }
}
