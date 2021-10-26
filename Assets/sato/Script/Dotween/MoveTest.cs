using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    [Header("�C�[�W���O�^�C�v�w��")]
    Ease easeTypes;

    [SerializeField]
    [Header("���[�v�^�C�v�w��")]
    LoopType loopTypes;

    [SerializeField]
    [Header("���[�v����������(-1�Ŗ������[�v)")]
    int loopTimes;

    [Header("�E�ړ�")]

    [SerializeField]
    [Header("�e�����ƕω��ʂ�ݒ�(���p��)")]
    bool moveFlag = false;
    [SerializeField]
    [Tooltip("�ړ���̈ʒu")]
    Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("���b�����Ĉړ������邩")]
    float moveTime = 0.0f;

    [SerializeField]
    [Header("�E�X�P�[��")]
    bool scaleFlag = false;
    [SerializeField]
    [Tooltip("�ω����������ړI�l")]
    float scaleRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    float scaleTime = 0.0f;

    [SerializeField]
    [Header("�E��]")]
    bool rotateFlag = false;
    [SerializeField]
    [Tooltip("��]���w��")]
    Vector3 rotateAxis = Vector3.up;
    [SerializeField]
    [Tooltip("��]�ʎw��")]
    float rotateRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    float rotateTime = 0.0f;

    [SerializeField]
    [Header("�E�J���[")]
    bool ColorFlag = false;
    [SerializeField]
    [Tooltip("Renderer������Γ����")]
    Renderer rendererComponent;
    [SerializeField]
    [Tooltip("image������Γ����")]
    Image image;
    [SerializeField]
    [Tooltip("�w�肵���F�ɕω�")]
    Color color = Color.red;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    float colorTime = 0.0f;

    [SerializeField]
    [Header("�E�t�F�[�h(�A���t�@�l�A���ʂ���)")]
    bool FadeFlag = false;
    [SerializeField, Range(0f,1f)]
    [Tooltip("�ω����������ړI�l(0�`1)")]
    float fadeRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    float fadeTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Sequence�̃C���X�^���X���쐬
        Sequence sequence = DOTween.Sequence();

        // �e����
        Moving();
        Scaling();
        Rotating();
        Coloring();
        Fading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //--------------------------------------------------
    // Moving
    // �ړ�����
    //--------------------------------------------------
    void Moving()
    {
        if (moveFlag)
        {
            // ���݈ʒu����ړ�
            transform.DOMove(moveRange, moveTime).SetRelative(true);
        }
    }

    //--------------------------------------------------
    // Scaling
    // �X�P�[������
    //--------------------------------------------------
    void Scaling()
    {
        if (scaleFlag)
        {
            // �X�P�[�����w�肵���A�j���[�V�����������Ȃ���ύX
            transform.DOScale(scaleRange, scaleTime).SetEase(easeTypes).SetLoops(loopTimes, loopTypes);
        }
    }

    //--------------------------------------------------
    // Rotating
    // ��]����
    //--------------------------------------------------
    void Rotating()
    {
        if (rotateFlag)
        {
            // �w�莲�����ɉ�]
            transform.DORotate(rotateAxis * rotateRange, rotateTime);
        }
    }

    //--------------------------------------------------
    // Coloring
    // �F�ύX����
    //--------------------------------------------------
    void Coloring()
    {
        if (ColorFlag)
        {
            if(rendererComponent != null)
            {
                // �w��F�ɏ��X�ɕψ�
                rendererComponent.material.DOColor(color, colorTime);
            }

            image.DOColor(color, colorTime);
        }
    }

    //--------------------------------------------------
    // Fading
    // �t�F�[�h����
    //--------------------------------------------------
    void Fading()
    {
        if(FadeFlag)
        {
            // ���X�Ɏw��l�ɕψ�
            image.DOFade(fadeRange, fadeTime);
        }
    }
}
