using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTextureWindowSizeScaler : MonoBehaviour
{
    Camera renderTextureCamera;

    // Start is called before the first frame update
    void Start()
    {
        renderTextureCamera = GetComponent<Camera>();
        renderTextureCamera.targetTexture.width = Screen.width;
        renderTextureCamera.targetTexture.height = Screen.height;
    }
}
