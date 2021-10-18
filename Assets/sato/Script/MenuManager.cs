using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("コントローラー番号")]
    public int controllerID = 98;

    [SerializeField]
    [Header("メニューオブジェクトを入れる")]
    GameObject Menu;

    bool onceFlag = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MenuValid();

        MenuInvalid();

        if(XInputManager.GetButtonRelease(controllerID, XButtonType.Start))
        {
            onceFlag = true;
        }
    }

    //--------------------------------------------------
    // MenuValid
    // メニュー画面有効化
    //--------------------------------------------------
    void MenuValid()
    {
        if (!Menu.activeSelf && onceFlag == true)
        {
            if (XInputManager.GetButtonTrigger(controllerID, XButtonType.Start))
            {
                Menu.SetActive(true);
                onceFlag = false;
            }
        }
    }

    //--------------------------------------------------
    // MenuInvalid
    // メニュー画面無効化
    //--------------------------------------------------
    void MenuInvalid()
    {
        if (Menu.activeSelf && onceFlag == true)
        {
            if (XInputManager.GetButtonTrigger(controllerID, XButtonType.Start))
            {
                Menu.SetActive(false);
                onceFlag = false;
            }
        }
    }

    //--------------------------------------------------
    // RoomSelect
    // ルームセレクトに遷移
    //--------------------------------------------------
    public void RoomSelect()
    {
        SceneManager.LoadScene("RoomSample");
    }

    //--------------------------------------------------
    // MenuCancel
    // メニュー画面無効化
    //--------------------------------------------------
    public void MenuCancel()
    {
        Menu.SetActive(false);
    }
}

