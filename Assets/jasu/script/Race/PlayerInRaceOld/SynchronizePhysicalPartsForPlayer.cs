using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizePhysicalPartsForPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject physicalParts;

    [SerializeField]
    FollowTargetInRace cameraFollow;

    [SerializeField]
    SynchronizeCamera synchronizeCamera;

    private void LateUpdate()
    {
        Vector3 worldPos = physicalParts.transform.position;
        transform.position = worldPos;
        physicalParts.transform.localPosition = Vector3.zero;

        // カメラ位置補正
        Vector3 offset = cameraFollow.GetOffset();
        cameraFollow.Follow(offset);

        synchronizeCamera.Synchronise();    // モニターカメラに同期
    }

}
