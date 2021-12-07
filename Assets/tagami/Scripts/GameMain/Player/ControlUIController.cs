using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIController : MonoBehaviour
{
    [SerializeField] RawImage buttonImage;
    [SerializeField] Texture keyButtonTexture;
    [SerializeField] Texture xinputButtonTexture;

    private void Start()
    {//初期設定
        UpdateImage();
    }

    private void Update()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        //XInputとKeyboardに対応する
        if (XInputManager.IsConnected(0))
        {
            buttonImage.texture = xinputButtonTexture;
        }
        else
        {
            buttonImage.texture = keyButtonTexture;
        }
    }
}
