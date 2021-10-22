using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera[] PreExecutionCameras;

    [SerializeField]
    static public List<CinemachineVirtualCamera> virtualCameraList = new List<CinemachineVirtualCamera>();

    private void Awake()
    {
        virtualCameraList.AddRange(PreExecutionCameras);
    }

    static public void SetActive(int _cameraNum, bool _activate)
    {
        virtualCameraList[_cameraNum].gameObject.SetActive(_activate);
    }

    static public void OnlyActive(int _cameraNum)
    {
        for(int i = 0; i < virtualCameraList.Count; i++)
        {
            if(i == _cameraNum)
                virtualCameraList[i].gameObject.SetActive(true);
            else
                virtualCameraList[i].gameObject.SetActive(false);
        }
    }

    public void OnlyActive(CinemachineVirtualCamera _vcam)
    {
        foreach(var vcam in virtualCameraList)
        {
            if (vcam.gameObject.GetInstanceID() == _vcam.gameObject.GetInstanceID())
                vcam.gameObject.SetActive(true);
            else
                vcam.gameObject.SetActive(false);
        }
    }
}
