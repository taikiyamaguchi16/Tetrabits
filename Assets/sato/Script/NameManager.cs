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
    // OnJoinedRoom
    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    //--------------------------------------------------
    public override void OnJoinedRoom()
    {
        inputField.SetActive(true);

        text.SetActive(true);

        nameInput.SetActive(true);
    }
}
