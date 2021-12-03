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
        buttonImage.texture = keyButtonTexture;
    }

    private void Update()
    {
        //XInputとKeyboardに対応する
        if (Input.anyKeyDown)
        {
            buttonImage.texture = keyButtonTexture;
        }
        if (XInputManager.GetAnyButtonTrigger(0))
        {
            buttonImage.texture = xinputButtonTexture;
        }
    }
}
