using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//トリガーエンター条件でVCam変更
[RequireComponent(typeof(Collider))]
public class VCameraSwitchSensor : MonoBehaviour
{
    [SerializeField] int enterVCamIndex = -1;
    [SerializeField] int exitVCamIndex = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (enterVCamIndex >= 0)
        {
            VirtualCameraManager.OnlyActive(enterVCamIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (exitVCamIndex >= 0)
        {
            VirtualCameraManager.OnlyActive(exitVCamIndex);
        }
    }
}
