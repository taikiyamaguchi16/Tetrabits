using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("InputField������")]
    GameObject inputField;

    [SerializeField]
    [Header("InputField�̒����ɂ���text������")]
    Text text;

    [SerializeField]
    [Header("��ʂ̈ē��I�u�W�F�N�g������")]
    GameObject TextObjects;

    [SerializeField]
    [Header("��\���ɂ������L�����o�X������")]
    GameObject deactiveObject;

    [SerializeField]
    [Header("���O���͌�Ɉړ��������V�[��")]
    SceneObject scene;

    [SerializeField]
    [Header("�f�o�b�O�p(true�Ŗ��O���͕K�{)")]
    bool debugFlag = false;

    private static Hashtable playerProps = new Hashtable();

    private void Awake()
    {
        // �J�X�^���v���p�e�B�̃L�[��ݒ�(�e�v���C���[����������isReady�̕ϐ��������Ă���C���[�W
        playerProps["isPlayerReady"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        playerProps.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //--------------------------------------------------
    // OnJoinedLobby
    // ���r�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //--------------------------------------------------
    //public override void OnJoinedLobby()
    //{
    //    inputField.SetActive(true);

    //    text.SetActive(true);

    //    nameInput.SetActive(true);
    //}

    //--------------------------------------------------
    // OnJoinedRoom
    // ���[���ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //--------------------------------------------------
    public override void OnJoinedRoom()
    {
        deactiveObject.SetActive(false);

        inputField.SetActive(true);

        TextObjects.SetActive(true);
    }

    //--------------------------------------------------
    // OnLeftRoom
    // ���[���ޏo���̃R�[���o�b�N
    //--------------------------------------------------
    public override void OnLeftRoom()
    {
        deactiveObject.SetActive(true);

        inputField.SetActive(false);

        TextObjects.SetActive(false);

        Debug.Log("���[������ޏo���܂���");
    }

    //--------------------------------------------------
    // NameInputExit
    // ���O�m�F���F���Ɏ��s
    //--------------------------------------------------
    public void NameInputExit()
    {
        // �v���C���[���g�̖��O��ݒ肷��
        PhotonNetwork.LocalPlayer.NickName = text.text;

        GameObject.Find("GameMainManager").GetComponent<GameInGameSwitcher>().RPCSwitchGameInGameScene(scene);
    }

    public bool DebugFlagName()
    {
        return debugFlag;
    }

    // �v���C���[�����[���ɓ������̏���
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
}
