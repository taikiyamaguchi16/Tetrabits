using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpRoundSeam : MonoBehaviour
{
    [SerializeField]
    RaceStageMolder raceStageMolder = null;

    [SerializeField, Tooltip("ワープのつなぎ目")]
    float warpPointZ = 50f;

    public float GetWarpPoint { get { return warpPointZ; } }

    // Update is called once per frame
    void Update()
    {
        if(raceStageMolder != null)
        {
            if(transform.localPosition.z >= raceStageMolder.transform.position.z + raceStageMolder.GetLaneLength + warpPointZ)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - raceStageMolder.GetLaneLength);
            }
        }
    }
}
