using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DOButtonSelected : MonoBehaviour
{
    [Header("ボタン")]
    [SerializeField] private GameObject[] Button;
    [Header("初期選択されているボタン")]
    [SerializeField] private GameObject FirstSelect;

    [Tooltip("コントローラー番号")]
    public int controllerID = 0;

    [SerializeField]
    [Tooltip("何秒かけて移動させるか")]
    float moveTime = 0.0f;

    // 現在選択中のボタンを判別する番号
    private int currentButtonNum;

    // ボタンの最大数を保存
    private int buttonMax;

    // 上下カーソル移動フラグ
    [SerializeField]
    [Header("カーソルを上下で動かすときにtrue")]
    bool isUpDown = false;

    // 左右カーソル移動フラグ
    [SerializeField]
    [Header("カーソルを左右で動かすときにtrue")]
    bool isLeftRight = false;

    [SerializeField]
    [Header("入力遅延時間設定")]
    float inputDelay = 0.5f;

    float delayTimer = 0.0f;

    bool isInput = false;

    [SerializeField]
    [Header("カーソル移動音")]
    AudioClip cursorMoveSe;

    //--------------------------------------------------
    // Start
    // 初期化
    //--------------------------------------------------
    void Start()
    {
        // イベントシステムの最初のボタンを設定
        if (EventSystem.current.currentSelectedGameObject != FirstSelect)
        {
            // イベントシステムのボタン設定をインスペクターで設定したボタンに設定する
            EventSystem.current.SetSelectedGameObject(FirstSelect);
        }

        // 設定されているボタンの数を格納
        buttonMax = Button.Length;

        for (int i = 0; i < Button.Length; i++)
        {
            // 同じ名前のやつを見つけてその番号を取得
            if (Button[i].name == FirstSelect.name)
            {
                currentButtonNum = i;
            }
        }
    }

    //--------------------------------------------------
    // Update
    // 更新（毎フレーム）
    //--------------------------------------------------
    void Update()
    {
        CursorMove();
        TimerCount();
    }

    //--------------------------------------------------
    // CursorMove
    // カーソルの移動処理
    //--------------------------------------------------
    void CursorMove()
    {
        // イベントシステムのボタン設定を現在のボタンに設定する
        EventSystem.current.SetSelectedGameObject(Button[currentButtonNum]);

        if (isUpDown)
        {
            if (!isInput)
            {
                // 上入力
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || XInputManager.GetThumbStickLeftY(controllerID) > 0)
                {
                    currentButtonNum--;

                    isInput = true;

                    SimpleAudioManager.PlayOneShot(cursorMoveSe);
                }

                // 下入力
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || XInputManager.GetThumbStickLeftY(controllerID) < 0)
                {
                    currentButtonNum++;

                    isInput = true;

                    SimpleAudioManager.PlayOneShot(cursorMoveSe);
                }
            }
        }

        if (isLeftRight)
        {
            if (!isInput)
            {
                // 左入力
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || XInputManager.GetThumbStickLeftX(controllerID) < 0)
                {
                    currentButtonNum--;

                    isInput = true;

                    SimpleAudioManager.PlayOneShot(cursorMoveSe);
                }

                // 上入力
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || XInputManager.GetThumbStickLeftX(controllerID) > 0)
                {
                    currentButtonNum++;

                    isInput = true;

                    SimpleAudioManager.PlayOneShot(cursorMoveSe);
                }
            }
        }

        // ボタンの最大値を超えないように
        if (currentButtonNum >= buttonMax)
        {
            currentButtonNum = 0;
        }

        // ボタンが0以下にならないように
        if (currentButtonNum < 0)
        {
            currentButtonNum = buttonMax - 1;
        }

        // 移動
        transform.DOMove(Button[currentButtonNum].transform.position, moveTime);
    }

    //--------------------------------------------------
    // TimerCount
    // 入力遅延用のタイマカウントアップ関数
    //--------------------------------------------------
    void TimerCount()
    {
        if (isInput)
        {
            delayTimer += Time.deltaTime;
        }

        if(delayTimer > inputDelay)
        {
            delayTimer = 0.0f;
            isInput = false;
        }
    }

    //--------------------------------------------------
    // GetCurrentButton
    // カーソルが指定しているボタンを返す
    //--------------------------------------------------
    public Button GetCurrentButton()
    {
        return Button[currentButtonNum].GetComponent<Button>();
    }
}
