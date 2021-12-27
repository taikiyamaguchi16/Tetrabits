using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : InputField
{
    NameManager nameManager;

    Text placeHolder;

    // ���O���͂��������ǂ���
    bool isNameInput = false;

    bool isNameVerify = false;

    // Start is called before the first frame update
    new void  Start()
    {
        // �e��R���|�[�l���g�擾
        nameManager = GameObject.Find("NameManager").GetComponent<NameManager>();

        placeHolder = GameObject.Find("Placeholder").GetComponent<Text>();

        text = null;

        placeHolder.text = "1�`10�����ȓ�";
        placeHolder.color = Color.black;
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

        if (string.IsNullOrEmpty(text) && nameManager.DebugFlagName())
        {
            ActivateInputField();

            placeHolder.text = "1�����ȏ���͂��Ă�������";
            placeHolder.color = Color.red;

            isNameInput = false;
            isNameVerify = false;
        }
    }

    // �S�I������
    protected override void LateUpdate()
    {
        base.LateUpdate();

        MoveTextEnd(false);
    }
}
