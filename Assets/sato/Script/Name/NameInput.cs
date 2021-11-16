using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NameInput : InputField
{
    NameManager nameManager;

    // 名前入力が完了かどうか
    bool isNameInput = false;

    bool isNameVerify = false;

    // Start is called before the first frame update
    new void  Start()
    {
        // 各種コンポーネント取得
        nameManager = GameObject.Find("NameManager").GetComponent<NameManager>();
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
    }

    // 全選択解除
    protected override void LateUpdate()
    {
        base.LateUpdate();

        MoveTextEnd(false);
    }
}
