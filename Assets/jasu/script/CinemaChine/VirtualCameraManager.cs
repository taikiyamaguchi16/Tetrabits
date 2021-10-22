using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera[] PreExecutionCameras;

    [SerializeField]
    static public List<CinemachineVirtualCamera> sVirtualCameraList = new List<CinemachineVirtualCamera>();

    private void Awake()
    {
        if(sVirtualCameraList.Count == 0)
            sVirtualCameraList.AddRange(PreExecutionCameras);
    }

    static public void SetActive(int _cameraNum, bool _activate)
    {
        sVirtualCameraList[_cameraNum].gameObject.SetActive(_activate);
    }

    static public void OnlyActive(int _cameraIndex)
    {
        for(int i = 0; i < sVirtualCameraList.Count; i++)
        {
            if(i == _cameraIndex)
                sVirtualCameraList[i].gameObject.SetActive(true);
            else
                sVirtualCameraList[i].gameObject.SetActive(false);
        }
    }

    static public void OnlyActive(CinemachineVirtualCamera _vcam)
    {
        foreach(var vcam in sVirtualCameraList)
        {
            if (vcam.gameObject.GetInstanceID() == _vcam.gameObject.GetInstanceID())
                vcam.gameObject.SetActive(true);
            else
                vcam.gameObject.SetActive(false);
        }
    }
}
