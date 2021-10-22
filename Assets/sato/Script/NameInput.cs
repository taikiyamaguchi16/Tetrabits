using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : MonoBehaviour
{
    [Header("InputFieldを入れる")]
    public InputField inputField;

    [Header("Nameを入れる")]
    public Text text;

    [SerializeField]
    [Header("NameManagerを入れる")]
    NameManager nameManager;

    [SerializeField]
    [Header("MatchMakingViewを入れる")]
    MatchmakingView matchmakingView;


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
        // InputFieldがアクティブならフォーカスを合わせる
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

        // エンターキーで入力強制終了
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputField.OnDeselect(new BaseEventData(EventSystem.current));
        }
    }

    // 入力確定後呼び出される
    public void EditText()
    {
        // プレイヤー自身の名前を設定する
        PhotonNetwork.NickName = text.text;

        // 入力確定後に消す
        nameManager.NameInputExit();

        // ボタンにカーソルのフォーカスを合わせる
        matchmakingView.GetComponent<CanvasCheck>().SetOnceFlag(true);
    }
}
