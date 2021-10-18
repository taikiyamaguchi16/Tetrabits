using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NameInput : MonoBehaviourPunCallbacks
{
    public InputField inputField;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        // 各種コンポーネント取得
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

    // 入力中呼び出される
    public void InputText()
    {
        //テキストにinputFieldの内容を反映
        text.text = inputField.text;
    }

    // 入力確定後呼び出される
    public void EditText()
    {
        // プレイヤー自身の名前を設定する
        PhotonNetwork.NickName = text.text;
    }
}
