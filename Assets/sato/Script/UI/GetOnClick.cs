using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOnClick : MonoBehaviour
{
    [SerializeField]
    [Header("カーソル入れる")]
    DOButtonSelected cursor;

    [SerializeField]
    [Header("コントーローラーID")]
    public int controllerID = 0;

    [SerializeField]
    [Header("カーソル決定音")]
    AudioClip cursorDecisionSe;

    [SerializeField]
    float Volume = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ButtonClickInput();
    }

    //--------------------------------------------------
    // ButtonClickInput
    // ボタン入力でカーソル位置のボタンを押す
    //--------------------------------------------------
    void ButtonClickInput()
    {
        // 決定キー
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || XInputManager.GetButtonTrigger(controllerID, XButtonType.B))
        {
            cursor.GetCurrentButton().onClick.Invoke();

            SimpleAudioManager.PlayOneShot(cursorDecisionSe, Volume);
        }
    }
}
