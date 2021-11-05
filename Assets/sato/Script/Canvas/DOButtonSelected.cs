using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DOButtonSelected : MonoBehaviour
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

    [SerializeField]
    [Tooltip("移動先の位置")]
    Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("何秒かけて移動させるか")]
    float moveTime = 0.0f;

    //--------------------------------------------------
    // Start
    // 初期化
    //--------------------------------------------------
    void Start()
    {

    }

    //--------------------------------------------------
    // Update
    // 更新（毎フレーム）
    //--------------------------------------------------
    void Update()
    {
        CursorMove();
    }

    //--------------------------------------------------
    // CursorMove
    // カーソルの移動処理
    //--------------------------------------------------
    void CursorMove()
    {
        // カーソル位置をイベントシステムで選択されている位置に指定
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            transform.DOMove(EventSystem.current.currentSelectedGameObject.transform.position, moveTime);
        }

        // 現在位置フラグがfalseの時
        if (currentFlag == false)
        {
            // 現在位置を更新
            currentPos = transform.position;
            currentFlag = true;
        }

        // 現在位置に変更が加われば
        if (currentPos != transform.position)
        {
            // 現在位置フラグをfalse
            currentFlag = false;
        }
    }
}
