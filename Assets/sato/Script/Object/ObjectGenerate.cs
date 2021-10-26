using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectGenerate : MonoBehaviourPunCallbacks
{
    // ��������I�u�W�F�N�g���w8��
    [SerializeField, Tooltip("�X�|�[��������I�u�W�F�N�g��ݒ�")]
    GameObject obj;

    // �������J�E���g�p
    int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjCreate();
    }

    //--------------------------------------------------
    // ObjCreate
    // �L�[���͂ŃI�u�W�F�N�g�𐶐�
    //--------------------------------------------------
    void ObjCreate()
    {
        // �G���^�[�L�[�Ő���
        if (Input.GetKey(KeyCode.Return))
        {
            GameObject spawned = PhotonNetwork.Instantiate(obj.name, Vector3.zero, Quaternion.identity);
            cnt++;
            Debug.Log("�������F" + cnt);
        }
    }
}
