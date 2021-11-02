using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DOManager : MonoBehaviour
{
    [SerializeField]
    [Header("�C�[�W���O�^�C�v�w��")]
    protected Ease easeTypes;

    [SerializeField]
    [Header("���[�v�^�C�v�w��")]
    protected LoopType loopTypes;

    [SerializeField]
    [Header("���[�v����������(-1�Ŗ������[�v)")]
    protected int loopTimes;

    [Header("�E�ړ�")]

    [SerializeField]
    [Header("�e�����ƕω��ʂ�ݒ�(���p��)")]
    protected bool moveFlag = false;
    [SerializeField]
    [Tooltip("�ړ���̈ʒu")]
    protected Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("���b�����Ĉړ������邩")]
    protected float moveTime = 0.0f;

    [SerializeField]
    [Header("�E�X�P�[��")]
    protected bool scaleFlag = false;
    [SerializeField]
    [Tooltip("�ω����������ړI�l")]
    protected float scaleRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    protected float scaleTime = 0.0f;

    [SerializeField]
    [Header("�E��]")]
    protected bool rotateFlag = false;
    [SerializeField]
    [Tooltip("��]���w��")]
    protected Vector3 rotateAxis = Vector3.up;
    [SerializeField]
    [Tooltip("��]�ʎw��")]
    protected float rotateRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    protected float rotateTime = 0.0f;

    [SerializeField]
    [Header("�E�J���[")]
    protected bool ColorFlag = false;
    [SerializeField]
    [Tooltip("Renderer������Γ����")]
    protected Renderer rendererComponent;
    [SerializeField]
    [Tooltip("image������Γ����")]
    protected Image image;
    [SerializeField]
    [Tooltip("�w�肵���F�ɕω�")]
    protected Color color = Color.red;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    protected float colorTime = 0.0f;

    [SerializeField]
    [Header("�E�t�F�[�h(�A���t�@�l�A���ʂ���)")]
    protected bool FadeFlag = false;
    [SerializeField, Range(0f, 1f)]
    [Tooltip("�ω����������ړI�l(0�`1)")]
    protected float fadeRange = 0.0f;
    [SerializeField]
    [Tooltip("���b�����ĕω������邩")]
    protected float fadeTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
