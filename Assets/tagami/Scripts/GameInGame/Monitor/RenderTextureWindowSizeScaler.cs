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
        //renderTextureCamera = GetComponent<Camera>();
        //renderTextureCamera.targetTexture.width = Screen.width;
        //renderTextureCamera.targetTexture.height = Screen.height;
        Debug.LogError("この機能は一時的に制限されています。使用しないでください。 2021/10/17");
    }
}
