using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Tooltip("�R���g���[���[�ԍ�")]
    public int controllerID = 0;

    [SerializeField]
    [Header("���j���[�I�u�W�F�N�g������")]
    GameObject Menu;

    [SerializeField]
    [Header("�V�[����")]
    private SceneObject scene;

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
        SceneManager.LoadScene(scene);
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

