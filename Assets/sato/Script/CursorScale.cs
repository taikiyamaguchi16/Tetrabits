using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScale : MonoBehaviour
{
    // �f�t�H���g�T�C�Y�w��
    private Vector3 scaleMin;

    // �ő�T�C�Y�w��
    private Vector3 scaleMax = new Vector3(0.0f, 0.0f, 0.0f);

    // �X�P�[�����g�k����ۂɉ����Z���s�����̒l
    [SerializeField][Header("�g�k���x")]
    private float scaleChangeSpeed = 0.1f;

    // �X�P�[���������Z���邩�Ǘ�����t���O(false = ���Z / true = ���Z)
    private bool scaleFlag = false;

    // �^�C�}�[���
    [SerializeField]
    [Header("�g�k�؂�ւ�����")]
    private float timerLimit = 1.0f;


    // �g�k�^�C�}�[
    private float timer = 0.0f;

    //--------------------------------------------------
    // Start
    // ������
    //--------------------------------------------------
    void Start()
    {
        // �V�[���J�n���̃X�P�[�����ŏ��l(�f�t�H���g�l)�Ƃ��ĕۑ�
        scaleMin = gameObject.transform.localScale;

        // �f�t�H���g�T�C�Y���Z
        scaleMax += scaleMin;

        // �f�t�H���g�T�C�Y�{�␳����
        scaleMax += new Vector3(0.3f, 1.5f, 0.0f);
    }

    //--------------------------------------------------
    // Update
    // �X�V�i���t���[���j
    //--------------------------------------------------
    void Update()
    {
        ScaleChange();
    }

    //--------------------------------------------------
    // ScaleChange
    // �J�[�\���̊g�k����
    //--------------------------------------------------
    private void ScaleChange()
    {
        timer += Time.deltaTime;

        // ���k
        if(scaleFlag == false)
        {
            // ��Ԃő傫����ύX
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleMin, scaleChangeSpeed);
        }

        // �g��
        else if(scaleFlag == true)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleMax, scaleChangeSpeed);

        }

        // ���k�J�n
        if(timer >= timerLimit && scaleFlag == true)
        {
            scaleFlag = false;
            timer = 0.0f;
        }

        // �g��J�n
        if(timer >= timerLimit && scaleFlag == false)
        {
            scaleFlag = true;
            timer = 0.0f;
        }
    }
}
