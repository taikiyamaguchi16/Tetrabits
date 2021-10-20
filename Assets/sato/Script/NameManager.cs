using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject inputField;

    [SerializeField]
    GameObject text;

    [SerializeField]
    GameObject nameInput;

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
    // ロビーへの接続が成功した時に呼ばれるコールバック
    //--------------------------------------------------
    public override void OnJoinedLobby()
    {
        inputField.SetActive(true);

        text.SetActive(true);

        nameInput.SetActive(true);
    }

    //--------------------------------------------------
    // OnJoinedRoom
    // ルームへの接続が成功した時に呼ばれるコールバック
    //--------------------------------------------------
    //public override void OnJoinedRoom()
    //{
    //    inputField.SetActive(true);

    //    text.SetActive(true);

    //    nameInput.SetActive(true);
    //}

    //--------------------------------------------------
    // NameInputExit
    // 名前入力完了後にInput系をすべて非表示
    //--------------------------------------------------
    public void NameInputExit()
    {
        inputField.SetActive(false);

        text.SetActive(false);

        nameInput.SetActive(false);
    }
}
