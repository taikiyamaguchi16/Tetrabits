using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//トリガーエンター条件でVCam変更
[RequireComponent(typeof(Collider))]
public class VCameraSwitchSensor : MonoBehaviour
{
    //[SerializeField] int enterVCamIndex = -1;
    [SerializeField] int stayVCamIndex = -1;
    [SerializeField] int exitVCamIndex = -1;

    int numCollidingObject = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numCollidingObject++;
            if (stayVCamIndex >= 0)
            {
                VirtualCameraManager.OnlyActive(stayVCamIndex);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numCollidingObject--;
            if (exitVCamIndex >= 0 && numCollidingObject <= 0)
            {
                VirtualCameraManager.OnlyActive(exitVCamIndex);
            }
        }
    }
}
