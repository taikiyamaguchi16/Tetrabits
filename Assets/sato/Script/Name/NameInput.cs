using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : InputField
{
    NameManager nameManager;

    // ���O���͂��������ǂ���
    bool isNameInput = false;

    bool isNameVerify = false;

    // Start is called before the first frame update
    new void  Start()
    {
        // �e��R���|�[�l���g�擾
        nameManager = GameObject.Find("NameManager").GetComponent<NameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // InputField���A�N�e�B�u�Ȃ�t�H�[�J�X�����킹��
        if (gameObject.activeSelf)
        {
            Select();
        }

        NameJudgment();
    }

    //--------------------------------------------------
    // NameJudgment
    // ���O���͂��I��点�邩�A�p�����邩
    //--------------------------------------------------
    void NameJudgment()
    {
        // ���O���͂��I�����ăV�[���؂�ւ�
        if (isNameVerify)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                nameManager.NameInputExit();
            }
        }

        // ���O���m�肷�邩�ύX���邩
        if (isNameInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isNameVerify = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ActivateInputField();

                isNameInput = false;
                isNameVerify = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            text = null;

            isNameInput = false;
            isNameVerify = false;

            PhotonNetwork.LeaveRoom();
        }
    }

    // ���͒��Ăяo�����
    public void InputText()
    {
        
    }

    // ���͊m���Ăяo�����
    public void EditText()
    {
        isNameInput = true;
    }

    // �S�I������
    protected override void LateUpdate()
    {
        base.LateUpdate();

        MoveTextEnd(false);
    }
}
