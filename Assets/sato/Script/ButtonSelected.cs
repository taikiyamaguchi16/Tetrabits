using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    [Header("�{�^��")]
    [SerializeField] private GameObject[] Button;
    [Header("�����I������Ă���{�^��")]
    [SerializeField] private GameObject FirstSelect;

    [Tooltip("�R���g���[���[�ԍ�")]
    public int controllerID = 99;

    // �J�[�\���ʒu���w�肷��ԍ�
    private int cursorNum;

    // �J�[�\���̓��͎��ԊǗ�
    private float delayTime = 0.0f;

    // ���݂̃J�[�\���ʒu
    private Vector3 currentPos;

    // ���݈ʒu�X�V�t���O
    private bool currentFlag = false;

    //--------------------------------------------------
    // Start
    // ������
    //--------------------------------------------------
    void Start()
    {
        // �J�[�\���ʒu���C�x���g�V�X�e���őI������Ă���ʒu�Ɏw��
        transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
    }

    //--------------------------------------------------
    // Update
    // �X�V�i���t���[���j
    //--------------------------------------------------
    void Update()
    {
        // �J�[�\���ʒu���C�x���g�V�X�e���őI������Ă���ʒu�Ɏw��
        transform.position = EventSystem.current.currentSelectedGameObject.transform.position;

        // ���݈ʒu�t���O��false�̎�
        if(currentFlag == false)
        {
            // ���݈ʒu���X�V
            currentPos = transform.position;
            currentFlag = true;
        }

        // ���݈ʒu�ɕύX��������
        if (currentPos != transform.position)
        {
            // ���݈ʒu�t���O��false�ASE�Đ�
            currentFlag = false;
        }
    }

    //--------------------------------------------------
    // PadInput
    // �p�b�h�̓��͋L�q
    //--------------------------------------------------
    private void PadInput()
    {
        // �����
        if (Input.GetKeyDown(KeyCode.W) || XInputManager.GetThumbStickLeftY(controllerID) > 0)
        {
            // ��ԉ��̎��ȊO
            if (cursorNum > 0)
            {
                cursorNum--;

                transform.position = Button[cursorNum].transform.position;
            }
        }

        // ������
        else if (Input.GetKeyDown(KeyCode.S) || XInputManager.GetThumbStickLeftY(controllerID) < 0)
        {
            // ��ԏ�̎��ȊO
            if (cursorNum < Button.Length - 1)
            {
                cursorNum++;

                transform.position = Button[cursorNum].transform.position;
            }
        }

        // �E���͂��ꂽ�Ƃ�
        else if (Input.GetKeyDown(KeyCode.D) || XInputManager.GetThumbStickLeftX(controllerID) > 0)
        {
            // �J�[�\���ʒu����ԉE�łȂ��Ƃ�
            if (cursorNum < Button.Length - 1)
            {
                cursorNum++;

                this.transform.position = Button[cursorNum].transform.position;
            }
        }

        // �����͂��ꂽ�Ƃ�
        else if (Input.GetKeyDown(KeyCode.A) || XInputManager.GetThumbStickLeftX(controllerID) < 0)
        {
            // �J�[�\���ʒu����ԉE�łȂ��Ƃ�
            if (cursorNum > 0)
            {
                cursorNum--;

                this.transform.position = Button[cursorNum].transform.position;
            }
        }
    }

    //--------------------------------------------------
    // InputTimeCount
    // �p�b�h�̓��͎���
    //--------------------------------------------------
    private void InputTimeCount()
    {
        // �^�C�}���Z
        delayTime += Time.deltaTime;

        if (delayTime > 0.2f)
        {
            delayTime = 0.0f;
        }
    }
}
