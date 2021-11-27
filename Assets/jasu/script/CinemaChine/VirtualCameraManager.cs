using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

public class VirtualCameraManager : MonoBehaviour
{
    [System.Serializable]
    public struct DepthOfFieldParameterForCameraStruct
    {
        public float forcusDistance;

        public float focalLength;
    }


    [SerializeField]
    CinemachineBrain cinemachineBrain = null;

    [SerializeField]
    GlobalVolumeController globalVolumeController = null;

    [SerializeField]
    CinemachineVirtualCamera[] PreExecutionCameras;
    
    static public List<CinemachineVirtualCamera> sVirtualCameraList = new List<CinemachineVirtualCamera>();

    [SerializeField]
    DepthOfFieldParameterForCameraStruct[] depthOfFieldParameterForCameras;

    static List<DepthOfFieldParameterForCameraStruct> sDepthOfFieldParameterForCameraList = new List<DepthOfFieldParameterForCameraStruct>();

    static bool sHavingGlobalVolumeController = false;

    static DepthOfFieldParameterForCameraStruct sActiveDepthOfFieldParam;

    static DepthOfFieldParameterForCameraStruct sOldDepthOfFieldParam;

    static bool isMovingCamera = false;
    
    float timer = 0f;

    bool firstUpdate = true;

    // デバッグ時インスペクターで値監視用
    //[SerializeField]
    //DepthOfFieldParameterForCameraStruct depthOfField;

    //[SerializeField]
    //float t;

    //[SerializeField]
    //DepthOfFieldParameterForCameraStruct activeDepthOfFieldParam;

    //[SerializeField]
    //DepthOfFieldParameterForCameraStruct oldDepthOfFieldParam;

    private void Awake()
    {
        if(cinemachineBrain == null)
        {
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        if(sVirtualCameraList.Count == 0)
        {
            sVirtualCameraList.AddRange(PreExecutionCameras);
        }

        if (sDepthOfFieldParameterForCameraList.Count == 0)
        {
            sDepthOfFieldParameterForCameraList.AddRange(depthOfFieldParameterForCameras);
        }

        if (globalVolumeController != null)
        {
            sHavingGlobalVolumeController = true;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        //activeDepthOfFieldParam = sActiveDepthOfFieldParam;
        //oldDepthOfFieldParam = sOldDepthOfFieldParam;

        if (firstUpdate)
        {
            firstUpdate = false;

            // 初期化
            OnlyActive(0);
            sOldDepthOfFieldParam = sActiveDepthOfFieldParam;
            globalVolumeController.depthOfField.focusDistance.value = sActiveDepthOfFieldParam.forcusDistance;
            globalVolumeController.depthOfField.focusDistance.value = sActiveDepthOfFieldParam.focalLength;
        }

        if (globalVolumeController != null && isMovingCamera)
        {
            timer += Time.deltaTime;
            if(timer >= cinemachineBrain.m_DefaultBlend.BlendTime)
            {
                isMovingCamera = false;
                timer = 0f;

                sOldDepthOfFieldParam = sActiveDepthOfFieldParam;

                globalVolumeController.depthOfField.focusDistance.value = sActiveDepthOfFieldParam.forcusDistance;
                globalVolumeController.depthOfField.focalLength.value = sActiveDepthOfFieldParam.focalLength;
            }
            else
            {
                //t = timer / cinemachineBrain.m_DefaultBlend.BlendTime;
                //depthOfField.forcusDistance = Mathf.Lerp(sOldDepthOfFieldParam.forcusDistance, sActiveDepthOfFieldParam.forcusDistance, timer / cinemachineBrain.m_DefaultBlend.BlendTime);
                //depthOfField.focalLength = Mathf.Lerp(sOldDepthOfFieldParam.focalLength, sActiveDepthOfFieldParam.focalLength, timer / cinemachineBrain.m_DefaultBlend.BlendTime);
                globalVolumeController.depthOfField.focusDistance.value = Mathf.Lerp(sOldDepthOfFieldParam.forcusDistance, sActiveDepthOfFieldParam.forcusDistance, timer / cinemachineBrain.m_DefaultBlend.BlendTime);
                globalVolumeController.depthOfField.focalLength.value = Mathf.Lerp(sOldDepthOfFieldParam.focalLength, sActiveDepthOfFieldParam.focalLength, timer / cinemachineBrain.m_DefaultBlend.BlendTime);
            }
        }
    }

    static public void SetActive(int _cameraIndex, bool _activate)
    {
        sVirtualCameraList[_cameraIndex].gameObject.SetActive(_activate);

        SetDepthOfFieldParameter(_cameraIndex);
    }

    static public void OnlyActive(int _cameraIndex)
    {
        for(int i = 0; i < sVirtualCameraList.Count; i++)
        {
            if(i == _cameraIndex)
            {
                sVirtualCameraList[i].gameObject.SetActive(true);

                SetDepthOfFieldParameter(i);
            }
            else
            {
                sVirtualCameraList[i].gameObject.SetActive(false);
            }
        }
    }

    static public void OnlyActive(CinemachineVirtualCamera _vcam)
    {
        for(int i = 0; i < sVirtualCameraList.Count; i++)
        {
            if (sVirtualCameraList[i].gameObject.GetInstanceID() == _vcam.gameObject.GetInstanceID())
            {
                sVirtualCameraList[i].gameObject.SetActive(true);

                SetDepthOfFieldParameter(i);
            }
            else
            {
                sVirtualCameraList[i].gameObject.SetActive(false);
            }
        }
    }

    static private void SetDepthOfFieldParameter(int _index)
    {
        if (sHavingGlobalVolumeController)
        {
            isMovingCamera = true;

            DepthOfFieldParameterForCamera depthOfFieldParameterForCamera = null;
            if ((depthOfFieldParameterForCamera = sVirtualCameraList[_index].gameObject.GetComponent<DepthOfFieldParameterForCamera>()) != null)
            {
                sActiveDepthOfFieldParam.forcusDistance = depthOfFieldParameterForCamera.forcusDistance;
                sActiveDepthOfFieldParam.focalLength = depthOfFieldParameterForCamera.focalLength;
            }
            else
            {
                sActiveDepthOfFieldParam.forcusDistance = sDepthOfFieldParameterForCameraList[_index].forcusDistance;
                sActiveDepthOfFieldParam.focalLength = sDepthOfFieldParameterForCameraList[_index].focalLength;
            }
        }
    }
}
