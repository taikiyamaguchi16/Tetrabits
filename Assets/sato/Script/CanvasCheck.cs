using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasCheck : MonoBehaviour
{
    // ステージ管理用のフラグ(コンポーネント呼び出しを複数回呼ぶなどを防ぐため)
    [SerializeField]
    private bool onceFlag = true;

    // パッドでボタン選択用
    Button[] selectButton;

    //--------------------------------------------------
    // OnEnable
    // アクティブ化したときに実行
    //--------------------------------------------------
    private void OnEnable()
    {
        // 連続処理を防ぐ
        onceFlag = true;
    }

    //--------------------------------------------------
    // OnDisable
    // 非アクティブ化したときに実行
    //--------------------------------------------------
    private void OnDisable()
    {
        // 連続処理を防ぐ
        onceFlag = true;
    }

    //--------------------------------------------------
    // Awake
    // Startより先に呼ばれる初期化
    //--------------------------------------------------
    private void Awake()
    {

    }

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
        FirstSelected();
    }

    //--------------------------------------------------
    // CanvasCheck
    // キャンバスチェック
    //--------------------------------------------------
    private void FirstSelected()
    {
        // 各ボタン押下時にtrueにして不必要な回数呼ばれるのを防ぐ
        if (onceFlag)
        {
            // SelectStageキャンバスがtrueの時
            if (gameObject.activeSelf)
            {
                // アタッチされたキャンバスの子のボタンコンポーネント取得
                selectButton = gameObject.GetComponentsInChildren<Button>();

                // 初期のカーソル位置のボタンを設定(暫定0)
                selectButton[0].Select();

                // フラグを折って次にtrueになるまでこの関数実行を防ぐ
                onceFlag = false;
            }
        }
    }

    //--------------------------------------------------
    // SetOnceFlag
    // 外部から変更するために使用
    //--------------------------------------------------
    public void SetOnceFlag(bool _onceflag)
    {
        onceFlag = _onceflag;
    }

    //--------------------------------------------------
    // GetselectButton
    // 外部に渡すための関数
    //--------------------------------------------------
    public Button[] GetselectButton()
    {
        return selectButton;
    }
}
