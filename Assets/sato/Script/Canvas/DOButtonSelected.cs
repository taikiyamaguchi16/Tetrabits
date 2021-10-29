using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DOButtonSelected : MonoBehaviour
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

    [SerializeField]
    [Tooltip("�ړ���̈ʒu")]
    Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("���b�����Ĉړ������邩")]
    float moveTime = 0.0f;

    //--------------------------------------------------
    // Start
    // ������
    //--------------------------------------------------
    void Start()
    {

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
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            transform.DOMove(EventSystem.current.currentSelectedGameObject.transform.position, moveTime);
        }

        // ���݈ʒu�t���O��false�̎�
        if (currentFlag == false)
        {
            // ���݈ʒu���X�V
            currentPos = transform.position;
            currentFlag = true;
        }

        // ���݈ʒu�ɕύX��������
        if (currentPos != transform.position)
        {
            // ���݈ʒu�t���O��false
            currentFlag = false;
        }
    }
}
