using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraTest : MonoBehaviour
{
    [SerializeField]
    int testNum;

    // Update is called once per frame
    void Update()
    {
        VirtualCameraManager.OnlyActive(testNum);
    }
}
