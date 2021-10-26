using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : MonoBehaviour
{
    [Header("InputField������")]
    public InputField inputField;

    [Header("Name������")]
    public Text text;

    [SerializeField]
    [Header("NameManager������")]
    NameManager nameManager;

    [SerializeField]
    [Header("MatchMakingView������")]
    MatchmakingView matchmakingView;


    // Start is called before the first frame update
    void Start()
    {
        // �e��R���|�[�l���g�擾
        inputField = inputField.GetComponent<InputField>();
        text = text.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // InputField���A�N�e�B�u�Ȃ�t�H�[�J�X�����킹��
        if (gameObject.activeSelf)
        {
            inputField.Select();
        }
    }

    // ���͒��Ăяo�����
    public void InputText()
    {
        //�e�L�X�g��inputField�̓��e�𔽉f
        text.text = inputField.text;

        // �G���^�[�L�[�œ��͋����I��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputField.OnDeselect(new BaseEventData(EventSystem.current));
        }
    }

    // ���͊m���Ăяo�����
    public void EditText()
    {
        // �v���C���[���g�̖��O��ݒ肷��
        PhotonNetwork.NickName = text.text;

        // ���͊m���ɏ���
        nameManager.NameInputExit();

        // �{�^���ɃJ�[�\���̃t�H�[�J�X�����킹��
        matchmakingView.GetComponent<CanvasCheck>().SetOnceFlag(true);
    }
}
