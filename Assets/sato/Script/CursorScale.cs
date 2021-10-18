using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScale : MonoBehaviour
{
    // デフォルトサイズ指定
    private Vector3 scaleMin;

    // 最大サイズ指定
    private Vector3 scaleMax = new Vector3(0.0f, 0.0f, 0.0f);

    // スケールを拡縮する際に加減算を行う元の値
    [SerializeField][Header("拡縮速度")]
    private float scaleChangeSpeed = 0.1f;

    // スケールを加減算するか管理するフラグ(false = 減算 / true = 加算)
    private bool scaleFlag = false;

    // タイマー上限
    [SerializeField]
    [Header("拡縮切り替え時間")]
    private float timerLimit = 1.0f;


    // 拡縮タイマー
    private float timer = 0.0f;

    //--------------------------------------------------
    // Start
    // 初期化
    //--------------------------------------------------
    void Start()
    {
        // シーン開始時のスケールを最小値(デフォルト値)として保存
        scaleMin = gameObject.transform.localScale;

        // デフォルトサイズ加算
        scaleMax += scaleMin;

        // デフォルトサイズ＋補正距離
        scaleMax += new Vector3(0.3f, 1.5f, 0.0f);
    }

    //--------------------------------------------------
    // Update
    // 更新（毎フレーム）
    //--------------------------------------------------
    void Update()
    {
        ScaleChange();
    }

    //--------------------------------------------------
    // ScaleChange
    // カーソルの拡縮処理
    //--------------------------------------------------
    private void ScaleChange()
    {
        timer += Time.deltaTime;

        // 収縮
        if(scaleFlag == false)
        {
            // 補間で大きさを変更
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleMin, scaleChangeSpeed);
        }

        // 拡大
        else if(scaleFlag == true)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scaleMax, scaleChangeSpeed);

        }

        // 収縮開始
        if(timer >= timerLimit && scaleFlag == true)
        {
            scaleFlag = false;
            timer = 0.0f;
        }

        // 拡大開始
        if(timer >= timerLimit && scaleFlag == false)
        {
            scaleFlag = true;
            timer = 0.0f;
        }
    }
}
