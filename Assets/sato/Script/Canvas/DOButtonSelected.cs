using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DOButtonSelected : MonoBehaviour
{
    [Header("�{�^��")]
    [SerializeField] private GameObject[] Button;
    [Header("�����I������Ă���{�^��")]
    [SerializeField] private GameObject FirstSelect;

    [Tooltip("�R���g���[���[�ԍ�")]
    public int controllerID = 99;

    [SerializeField]
    [Tooltip("���b�����Ĉړ������邩")]
    float moveTime = 0.0f;

    // ���ݑI�𒆂̃{�^���𔻕ʂ���ԍ�
    private int currentButtonNum;

    // �{�^���̍ő吔��ۑ�
    private int buttonMax;

    // �㉺�J�[�\���ړ��t���O
    [SerializeField]
    [Header("�J�[�\�����㉺�œ������Ƃ���true")]
    bool isUpDown = false;

    // ���E�J�[�\���ړ��t���O
    [SerializeField]
    [Header("�J�[�\�������E�œ������Ƃ���true")]
    bool isLeftRight = false;

    //--------------------------------------------------
    // Start
    // ������
    //--------------------------------------------------
    void Start()
    {
        // �C�x���g�V�X�e���̍ŏ��̃{�^����ݒ�
        if (EventSystem.current.currentSelectedGameObject != FirstSelect)
        {
            // �C�x���g�V�X�e���̃{�^���ݒ���C���X�y�N�^�[�Őݒ肵���{�^���ɐݒ肷��
            EventSystem.current.SetSelectedGameObject(FirstSelect);
        }

        // �ݒ肳��Ă���{�^���̐����i�[
        buttonMax = Button.Length;

        for (int i = 0; i < Button.Length; i++)
        {
            // �������O�̂�������Ă��̔ԍ����擾
            if (Button[i].name == FirstSelect.name)
            {
                currentButtonNum = i;
            }
        }
    }

    //--------------------------------------------------
    // Update
    // �X�V�i���t���[���j
    //--------------------------------------------------
    void Update()
    {
        CursorMove();
    }

    //--------------------------------------------------
    // CursorMove
    // �J�[�\���̈ړ�����
    //--------------------------------------------------
    void CursorMove()
    {
        // �J�[�\���ʒu���C�x���g�V�X�e���őI������Ă���ʒu�Ɏw��
        //if (EventSystem.current.currentSelectedGameObject != null)
        //{
        //    transform.DOMove(EventSystem.current.currentSelectedGameObject.transform.position, moveTime);
        //}

        // �C�x���g�V�X�e���̃{�^���ݒ�����݂̃{�^���ɐݒ肷��
        EventSystem.current.SetSelectedGameObject(Button[currentButtonNum]);

        if (isUpDown)
        {
            // �����
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || XInputManager.GetThumbStickLeftY(controllerID) < 0)
            {
                currentButtonNum--;
            }

            // ������
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || XInputManager.GetThumbStickLeftY(controllerID) > 0)
            {
                currentButtonNum++;
            }
        }

        if (isLeftRight)
        {
            // ������
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || XInputManager.GetThumbStickLeftX(controllerID) < 0)
            {
                currentButtonNum--;
            }

            // �����
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || XInputManager.GetThumbStickLeftX(controllerID) > 0)
            {
                currentButtonNum++;
            }
        }

        // �{�^���̍ő�l�𒴂��Ȃ��悤��
        if (currentButtonNum >= buttonMax)
        {
            currentButtonNum = 0;
        }

        // �{�^����0�����ɂȂ�Ȃ��悤��
        if (currentButtonNum < 0)
        {
            currentButtonNum = buttonMax - 1;
        }

        // �ړ�
        transform.DOMove(Button[currentButtonNum].transform.position, moveTime);
    }

    //--------------------------------------------------
    // GetCurrentButton
    // �J�[�\�����w�肵�Ă���{�^����Ԃ�
    //--------------------------------------------------
    public Button GetCurrentButton()
    {
        return Button[currentButtonNum].GetComponent<Button>();
    }
}
