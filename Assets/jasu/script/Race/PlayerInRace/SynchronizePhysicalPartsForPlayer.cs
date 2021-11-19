using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizePhysicalPartsForPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject physicalParts;

    [SerializeField]
    FollowTarget cameraFollow;

    [SerializeField]
    SynchronizeCamera synchronizeCamera;

    private void LateUpdate()
    {
        Vector3 worldPos = physicalParts.transform.position;
        transform.position = worldPos;
        physicalParts.transform.localPosition = Vector3.zero;

        cameraFollow.Follow();  // カメラ位置補正

        synchronizeCamera.Synchronise();    // モニターカメラに同期
    }

}
