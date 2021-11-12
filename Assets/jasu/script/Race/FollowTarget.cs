using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    Transform followTrans;

    // シーン上に配置したこのオブジェクトとfollowTransとの距離
    Vector3 offset;

    [SerializeField]
    bool followX = true;

    [SerializeField]
    bool followY = true;

    [SerializeField]
    bool followZ = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - followTrans.position;
    }

    // Update is called once per frame
    void Update()
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
}
