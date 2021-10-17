using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("�R���g���[���[�ԍ�")]
    public int controllerID = 98;

    [SerializeField]
    [Header("���j���[�I�u�W�F�N�g������")]
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
    // ���j���[��ʗL����
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
    // ���j���[��ʖ�����
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
    // ���[���Z���N�g�ɑJ��
    //--------------------------------------------------
    public void RoomSelect()
    {
        SceneManager.LoadScene("RoomSample");
    }

    //--------------------------------------------------
    // MenuCancel
    // ���j���[��ʖ�����
    //--------------------------------------------------
    public void MenuCancel()
    {
        Menu.SetActive(false);
    }
}

