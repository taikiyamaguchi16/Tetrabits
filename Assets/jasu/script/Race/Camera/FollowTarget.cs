using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    Transform followTrans;

    // シーン上に配置したこのオブジェクトとfollowTransとの距離
    [SerializeField]
    Vector3 offset;

    public Vector3 GetOffset() { return offset; }

    [SerializeField]
    bool followX = true;

    [SerializeField]
    bool followY = true;

    [SerializeField]
    bool followZ = true;

    [SerializeField]
    bool self = false;

    [SerializeField]
    bool local = false;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.localPosition - followTrans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (self)
        {
            if (local)
            {
                Vector3 pos = transform.position;
                pos.z = followTrans.position.z + 20f;
                transform.position = pos;
            }
            else
            {
                Follow();
            }
        }
    }

    public void Follow()
    {
        Vector3 pos = offset;
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
