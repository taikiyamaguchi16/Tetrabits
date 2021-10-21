using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraAction : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("�����������J������ݒ�")]
    GameObject targetCamera;

    // �����ʒu
    [SerializeField]
    [Header("�����ʒu�����")]
    Vector3 startPos;

    // �����ʒu�����݂̃J�����̈ʒu�Őݒ肵�����ꍇ�Ɏg�p
    [SerializeField]
    [Header("�����ʒu�����݂̃J�����|�W�V�����Ɏ����ݒ�")]
    [Tooltip("true�Ŏ����Afalse�Ń}�j���A��")]
    bool autoFlag;

    // �����������ʒu
    [SerializeField]
    [Header("�����������ʒu�����")]
    Vector3 endPos;

    // ����l��
    [SerializeField]
    [Header("����l����ݒ�")]
    int numUpperLimit = 2;

    [SerializeField]
    [Header("�����l����ݒ�")]
    int numLowerLimit = 2;

    // �X�s�[�h
    [SerializeField]
    [Header("��ԃX�s�[�h")]
    float speed = 1.0f;

    // ���`��Ԃ����`��Ԃ���؂�ւ���
    [SerializeField]
    [Header("���`��Ԃ����`��Ԃ��؂�ւ�")]
    [Tooltip("true�Ő��`�Afalse�ŋ��`")]
    bool interpolateSwitch;

    // ��_�Ԃ̋���
    float distance;

    // �␳�������ǂ���
    [SerializeField]
    bool isInterplate = false;

    // Start is called before the first frame update
    void Start()
    {
        // �����ݒ�
        if (autoFlag)
        {
            // �J�����̏����ʒu��ۑ�
            startPos = targetCamera.transform.position;
        }
        else
        {
            targetCamera.transform.position = startPos;
        }

        // ��_�Ԃ̋������v�Z
        distance = Vector3.Distance(startPos, endPos);
    }

    // Update is called once per frame
    void Update()
    {
        InterpolateCamera();
    }

    //--------------------------------------------------
    // InterpolateCamera
    // �J������Ԋ֐�
    //--------------------------------------------------
    void InterpolateCamera()
    {
        if (isInterplate)
        {
            // ���݈ʒu
            float presentPos = (Time.time * speed) / distance;

            // ���`���
            if (interpolateSwitch)
            {
                // �J�����̈ړ�
                targetCamera.transform.position = Vector3.Lerp(startPos, endPos, presentPos);
            }
            // ���`���
            else
            {
                // �J�����̈ړ�
                targetCamera.transform.position = Vector3.Slerp(startPos, endPos, presentPos);
            }

            // �ݒ�ʒu�܂ňړ������ŕ�ԏI��
            if(targetCamera.transform.position.x >= endPos.x &&
                targetCamera.transform.position.y >= endPos.y &&
                targetCamera.transform.position.z >= endPos.z)
            {
                isInterplate = false;
            }
        }
    }

    //--------------------------------------------------
    // OnPlayerEnteredRoom
    // ���[���Ƀv���C���[�����������ۂɌĂ΂��R�[���o�b�N
    //--------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("liauhguglaouglluirhgalhguaigli");
        // �ݒ肵���l���ȏ�ɂȂ��
        if(PhotonNetwork.PlayerList.Length <= numUpperLimit && PhotonNetwork.PlayerList.Length >= numLowerLimit)
        {
            isInterplate = true;
        }
    }
}
