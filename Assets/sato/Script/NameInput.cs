using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : MonoBehaviour
{
    public InputField inputField;
    public Text text;

    [SerializeField]
    NameManager nameManager;

    [SerializeField]
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
    }

    // ���͊m���Ăяo�����
    public void EditText()
    {
        // �v���C���[���g�̖��O��ݒ肷��
        PhotonNetwork.NickName = text.text;

        // ���͊m���ɏ���
        nameManager.NameInputExit();

        matchmakingView.GetComponent<CanvasCheck>().SetOnceFlag(true);
    }
}
