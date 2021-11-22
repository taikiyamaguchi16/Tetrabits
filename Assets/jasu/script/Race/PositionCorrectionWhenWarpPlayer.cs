using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrectionWhenWarpPlayer : MonoBehaviour
{
    [SerializeField]
    public GameObject player = null;

    float warpPoint;

    [SerializeField]
    public RaceStageMolder raceStageMolder = null;

    float laneLength;

    [SerializeField]
    float playerCameraRange = 50f;

    // Start is called before the first frame update
    void Start()
    {
        warpPoint = player.GetComponent<WarpRoundSeam>().GetWarpPoint;
        laneLength = raceStageMolder.GetLaneLength;
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーがワープ地点の入り口付近
        if(player.transform.localPosition.z + playerCameraRange > raceStageMolder.transform.position.z + laneLength + warpPoint - playerCameraRange ||
            player.transform.localPosition.z - playerCameraRange > raceStageMolder.transform.position.z + laneLength + warpPoint + playerCameraRange)
        {
            // 出口付近にいたならプレイヤーのいる入り口付近に補正
            if(transform.localPosition.z > raceStageMolder.transform.position.z + warpPoint - playerCameraRange &&
                transform.localPosition.z < raceStageMolder.transform.position.z + warpPoint + playerCameraRange)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + laneLength);
            }

            // 範囲外でワープ
            if (transform.localPosition.z >= raceStageMolder.transform.position.z + laneLength + warpPoint + playerCameraRange)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - laneLength);
            }

        }   // プレイヤーがワープ地点の入り口付近
        else if (player.transform.localPosition.z + playerCameraRange > raceStageMolder.transform.position.z + warpPoint - playerCameraRange ||
            player.transform.localPosition.z - playerCameraRange > raceStageMolder.transform.position.z + warpPoint + playerCameraRange)
        {
            // 入り口付近にいたならプレイヤーのいる出口付近に補正
            if (transform.localPosition.z > raceStageMolder.transform.position.z + laneLength + warpPoint - playerCameraRange &&
                transform.localPosition.z < raceStageMolder.transform.position.z + laneLength + warpPoint + playerCameraRange)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - laneLength);
            }
        }
        else
        {
            if (transform.localPosition.z >= raceStageMolder.transform.position.z + laneLength + warpPoint)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - laneLength);
            }
        }
    }
}
