using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    [Header("ボタン")]
    [SerializeField] private GameObject[] Button;
    [Header("初期選択されているボタン")]
    [SerializeField] private GameObject FirstSelect;

    [Tooltip("コントローラー番号")]
    public int controllerID = 99;

    // カーソル位置を指定する番号
    private int cursorNum;

    // カーソルの入力時間管理
    private float delayTime = 0.0f;

    // 現在のカーソル位置
    private Vector3 currentPos;

    // 現在位置更新フラグ
    private bool currentFlag = false;

    //--------------------------------------------------
    // Start
    // 初期化
    //--------------------------------------------------
    void Start()
    {
        // カーソル位置をイベントシステムで選択されている位置に指定
        transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
    }

    //--------------------------------------------------
    // Update
    // 更新（毎フレーム）
    //--------------------------------------------------
    void Update()
    {
        // カーソル位置をイベントシステムで選択されている位置に指定
        transform.position = EventSystem.current.currentSelectedGameObject.transform.position;

        // 現在位置フラグがfalseの時
        if(currentFlag == false)
        {
            // 現在位置を更新
            currentPos = transform.position;
            currentFlag = true;
        }

        // 現在位置に変更が加われば
        if (currentPos != transform.position)
        {
            // 現在位置フラグをfalse、SE再生
            currentFlag = false;
        }
    }

    //--------------------------------------------------
    // PadInput
    // パッドの入力記述
    //--------------------------------------------------
    private void PadInput()
    {
        // 上入力
        if (Input.GetKeyDown(KeyCode.W) || XInputManager.GetThumbStickLeftY(controllerID) > 0)
        {
            // 一番下の時以外
            if (cursorNum > 0)
            {
                cursorNum--;

                transform.position = Button[cursorNum].transform.position;
            }
        }

        // 下入力
        else if (Input.GetKeyDown(KeyCode.S) || XInputManager.GetThumbStickLeftY(controllerID) < 0)
        {
            // 一番上の時以外
            if (cursorNum < Button.Length - 1)
            {
                cursorNum++;

                transform.position = Button[cursorNum].transform.position;
            }
        }

        // 右入力されたとき
        else if (Input.GetKeyDown(KeyCode.D) || XInputManager.GetThumbStickLeftX(controllerID) > 0)
        {
            // カーソル位置が一番右でないとき
            if (cursorNum < Button.Length - 1)
            {
                cursorNum++;

                this.transform.position = Button[cursorNum].transform.position;
            }
        }

        // 左入力されたとき
        else if (Input.GetKeyDown(KeyCode.A) || XInputManager.GetThumbStickLeftX(controllerID) < 0)
        {
            // カーソル位置が一番右でないとき
            if (cursorNum > 0)
            {
                cursorNum--;

                this.transform.position = Button[cursorNum].transform.position;
            }
        }
    }

    //--------------------------------------------------
    // InputTimeCount
    // パッドの入力時間
    //--------------------------------------------------
    private void InputTimeCount()
    {
        // タイマ加算
        delayTime += Time.deltaTime;

        if (delayTime > 0.2f)
        {
            delayTime = 0.0f;
        }
    }
}
