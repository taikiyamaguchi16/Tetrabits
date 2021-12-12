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

    [System.Serializable]
    public struct VirtualCameraWithDepthOfFieldStruct
    {
        public CinemachineVirtualCamera cinemachineVirtualCamera;

        public DepthOfFieldParameterForCameraStruct depthOfFieldParameter;
    }

    [SerializeField]
    CinemachineBrain cinemachineBrain = null;

    [SerializeField]
    static CinemachineImpulseSource sImpulseSource = null;

    [SerializeField]
    GlobalVolumeController globalVolumeController = null;

    //[SerializeField]
    //CinemachineVirtualCamera[] PreExecutionCameras;

    //static public List<CinemachineVirtualCamera> sVirtualCameraList = new List<CinemachineVirtualCamera>();

    //[SerializeField]
    //DepthOfFieldParameterForCameraStruct[] depthOfFieldParameterForCameras;

    //static List<DepthOfFieldParameterForCameraStruct> sDepthOfFieldParameterForCameraList = new List<DepthOfFieldParameterForCameraStruct>();

    [SerializeField]
    VirtualCameraWithDepthOfFieldStruct[] virtualCameraWithDepthOfFields;

    static List<VirtualCameraWithDepthOfFieldStruct> sVirtualCameraWithDepthOfFieldList = new List<VirtualCameraWithDepthOfFieldStruct>();

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
        if (cinemachineBrain == null)
        {
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        if(sImpulseSource == null)
        {
            sImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        if(sVirtualCameraWithDepthOfFieldList.Count != 0)
        {
            sVirtualCameraWithDepthOfFieldList.Clear();
        }
        sVirtualCameraWithDepthOfFieldList.AddRange(virtualCameraWithDepthOfFields);

        if (globalVolumeController != null)
        {
            sHavingGlobalVolumeController = true;
        }

        isMovingCamera = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ImpulseNoise();
        }
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
            if (timer >= cinemachineBrain.m_DefaultBlend.BlendTime)
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
        sVirtualCameraWithDepthOfFieldList[_cameraIndex].cinemachineVirtualCamera.gameObject.SetActive(_activate);

        SetDepthOfFieldParameter(_cameraIndex);
    }

    static public void OnlyActive(int _cameraIndex)
    {
        for (int i = 0; i < sVirtualCameraWithDepthOfFieldList.Count; i++)
        {
            if (i == _cameraIndex)
            {
                sVirtualCameraWithDepthOfFieldList[i].cinemachineVirtualCamera.gameObject.SetActive(true);

                SetDepthOfFieldParameter(i);
            }
            else
            {
                sVirtualCameraWithDepthOfFieldList[i].cinemachineVirtualCamera.gameObject.SetActive(false);
            }
        }
    }

    static public void OnlyActive(CinemachineVirtualCamera _vcam)
    {
        for (int i = 0; i < sVirtualCameraWithDepthOfFieldList.Count; i++)
        {
            if (sVirtualCameraWithDepthOfFieldList[i].cinemachineVirtualCamera.gameObject.GetInstanceID() == _vcam.gameObject.GetInstanceID())
            {
                sVirtualCameraWithDepthOfFieldList[i].cinemachineVirtualCamera.gameObject.SetActive(true);

                SetDepthOfFieldParameter(i);
            }
            else
            {
                sVirtualCameraWithDepthOfFieldList[i].cinemachineVirtualCamera.gameObject.SetActive(false);
            }
        }
    }

    static private void SetDepthOfFieldParameter(int _index)
    {
        if (sHavingGlobalVolumeController)
        {
            isMovingCamera = true;

            DepthOfFieldParameterForCamera depthOfFieldParameterForCamera = null;
            if ((depthOfFieldParameterForCamera = sVirtualCameraWithDepthOfFieldList[_index].cinemachineVirtualCamera.gameObject.GetComponent<DepthOfFieldParameterForCamera>()) != null)
            {
                sActiveDepthOfFieldParam.forcusDistance = depthOfFieldParameterForCamera.forcusDistance;
                sActiveDepthOfFieldParam.focalLength = depthOfFieldParameterForCamera.focalLength;
            }
            else
            {
                sActiveDepthOfFieldParam.forcusDistance = sVirtualCameraWithDepthOfFieldList[_index].depthOfFieldParameter.forcusDistance;
                sActiveDepthOfFieldParam.focalLength = sVirtualCameraWithDepthOfFieldList[_index].depthOfFieldParameter.focalLength;
            }
        }
    }

    static public void ImpulseNoise()
    {
        sImpulseSource.GenerateImpulse();
    }
}