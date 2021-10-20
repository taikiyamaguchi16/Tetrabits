using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasCheck : MonoBehaviour
{
    // �X�e�[�W�Ǘ��p�̃t���O(�R���|�[�l���g�Ăяo���𕡐���ĂԂȂǂ�h������)
    [SerializeField]
    private bool onceFlag = true;

    // �p�b�h�Ń{�^���I��p
    Button[] selectButton;

    //--------------------------------------------------
    // OnEnable
    // �A�N�e�B�u�������Ƃ��Ɏ��s
    //--------------------------------------------------
    private void OnEnable()
    {
        // �A��������h��
        onceFlag = true;
    }

    //--------------------------------------------------
    // OnDisable
    // ��A�N�e�B�u�������Ƃ��Ɏ��s
    //--------------------------------------------------
    private void OnDisable()
    {
        // �A��������h��
        onceFlag = true;
    }

    //--------------------------------------------------
    // Awake
    // Start����ɌĂ΂�鏉����
    //--------------------------------------------------
    private void Awake()
    {

    }

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
        FirstSelected();
    }

    //--------------------------------------------------
    // CanvasCheck
    // �L�����o�X�`�F�b�N
    //--------------------------------------------------
    private void FirstSelected()
    {
        // �e�{�^����������true�ɂ��ĕs�K�v�ȉ񐔌Ă΂��̂�h��
        if (onceFlag)
        {
            // SelectStage�L�����o�X��true�̎�
            if (gameObject.activeSelf)
            {
                // �A�^�b�`���ꂽ�L�����o�X�̎q�̃{�^���R���|�[�l���g�擾
                selectButton = gameObject.GetComponentsInChildren<Button>();

                // �����̃J�[�\���ʒu�̃{�^����ݒ�(�b��0)
                selectButton[0].Select();

                // �t���O��܂��Ď���true�ɂȂ�܂ł��̊֐����s��h��
                onceFlag = false;
            }
        }
    }

    //--------------------------------------------------
    // SetOnceFlag
    // �O������ύX���邽�߂Ɏg�p
    //--------------------------------------------------
    public void SetOnceFlag(bool _onceflag)
    {
        onceFlag = _onceflag;
    }

    //--------------------------------------------------
    // GetselectButton
    // �O���ɓn�����߂̊֐�
    //--------------------------------------------------
    public Button[] GetselectButton()
    {
        return selectButton;
    }
}
