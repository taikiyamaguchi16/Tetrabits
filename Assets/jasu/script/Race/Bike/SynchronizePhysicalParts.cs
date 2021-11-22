using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizePhysicalParts : MonoBehaviour
{
    [SerializeField]
    GameObject physicalParts;

    private void LateUpdate()
    {
        Vector3 worldPos = physicalParts.transform.position;
        transform.position = worldPos;
        physicalParts.transform.localPosition = Vector3.zero;
    }

}
