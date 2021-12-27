using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : InputField
{
    NameManager nameManager;

    Text placeHolder;

    // 名前入力が完了かどうか
    bool isNameInput = false;

    bool isNameVerify = false;

    // Start is called before the first frame update
    new void  Start()
    {
        // 各種コンポーネント取得
        nameManager = GameObject.Find("NameManager").GetComponent<NameManager>();

        placeHolder = GameObject.Find("Placeholder").GetComponent<Text>();

        text = null;

        placeHolder.text = "1～10文字以内";
        placeHolder.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        // InputFieldがアクティブならフォーカスを合わせる
        if (gameObject.activeSelf)
        {
            Select();
        }

        NameJudgment();
    }

    //--------------------------------------------------
    // NameJudgment
    // 名前入力を終わらせるか、継続するか
    //--------------------------------------------------
    void NameJudgment()
    {
        // 名前入力を終了してシーン切り替え
        if (isNameVerify)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                nameManager.NameInputExit();
            }
        }

        // 名前を確定するか変更するか
        if (isNameInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isNameVerify = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ActivateInputField();

                isNameInput = false;
                isNameVerify = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            text = null;

            isNameInput = false;
            isNameVerify = false;

            PhotonNetwork.LeaveRoom();
        }
    }

    // 入力中呼び出される
    public void InputText()
    {
        
    }

    // 入力確定後呼び出される
    public void EditText()
    {
        isNameInput = true;

        if (string.IsNullOrEmpty(text) && nameManager.DebugFlagName())
        {
            ActivateInputField();

            placeHolder.text = "1文字以上入力してください";
            placeHolder.color = Color.red;

            isNameInput = false;
            isNameVerify = false;
        }
    }

    // 全選択解除
    protected override void LateUpdate()
    {
        base.LateUpdate();

        MoveTextEnd(false);
    }
}
