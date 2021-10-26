using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    [Header("イージングタイプ指定")]
    Ease easeTypes;

    [SerializeField]
    [Header("ループタイプ指定")]
    LoopType loopTypes;

    [SerializeField]
    [Header("ループさせたい回数(-1で無限ループ)")]
    int loopTimes;

    [Header("・移動")]

    [SerializeField]
    [Header("各挙動と変化量を設定(併用可)")]
    bool moveFlag = false;
    [SerializeField]
    [Tooltip("移動先の位置")]
    Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("何秒かけて移動させるか")]
    float moveTime = 0.0f;

    [SerializeField]
    [Header("・スケール")]
    bool scaleFlag = false;
    [SerializeField]
    [Tooltip("変化させたい目的値")]
    float scaleRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    float scaleTime = 0.0f;

    [SerializeField]
    [Header("・回転")]
    bool rotateFlag = false;
    [SerializeField]
    [Tooltip("回転軸指定")]
    Vector3 rotateAxis = Vector3.up;
    [SerializeField]
    [Tooltip("回転量指定")]
    float rotateRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    float rotateTime = 0.0f;

    [SerializeField]
    [Header("・カラー")]
    bool ColorFlag = false;
    [SerializeField]
    [Tooltip("Rendererがあれば入れる")]
    Renderer rendererComponent;
    [SerializeField]
    [Tooltip("imageがあれば入れる")]
    Image image;
    [SerializeField]
    [Tooltip("指定した色に変化")]
    Color color = Color.red;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    float colorTime = 0.0f;

    [SerializeField]
    [Header("・フェード(アルファ値、音量も可)")]
    bool FadeFlag = false;
    [SerializeField, Range(0f,1f)]
    [Tooltip("変化させたい目的値(0～1)")]
    float fadeRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    float fadeTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Sequenceのインスタンスを作成
        Sequence sequence = DOTween.Sequence();

        // 各挙動
        Moving();
        Scaling();
        Rotating();
        Coloring();
        Fading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //--------------------------------------------------
    // Moving
    // 移動処理
    //--------------------------------------------------
    void Moving()
    {
        if (moveFlag)
        {
            // 現在位置から移動
            transform.DOMove(moveRange, moveTime).SetRelative(true);
        }
    }

    //--------------------------------------------------
    // Scaling
    // スケール処理
    //--------------------------------------------------
    void Scaling()
    {
        if (scaleFlag)
        {
            // スケールを指定したアニメーションをさせながら変更
            transform.DOScale(scaleRange, scaleTime).SetEase(easeTypes).SetLoops(loopTimes, loopTypes);
        }
    }

    //--------------------------------------------------
    // Rotating
    // 回転処理
    //--------------------------------------------------
    void Rotating()
    {
        if (rotateFlag)
        {
            // 指定軸方向に回転
            transform.DORotate(rotateAxis * rotateRange, rotateTime);
        }
    }

    //--------------------------------------------------
    // Coloring
    // 色変更処理
    //--------------------------------------------------
    void Coloring()
    {
        if (ColorFlag)
        {
            if(rendererComponent != null)
            {
                // 指定色に徐々に変移
                rendererComponent.material.DOColor(color, colorTime);
            }

            image.DOColor(color, colorTime);
        }
    }

    //--------------------------------------------------
    // Fading
    // フェード処理
    //--------------------------------------------------
    void Fading()
    {
        if(FadeFlag)
        {
            // 徐々に指定値に変移
            image.DOFade(fadeRange, fadeTime);
        }
    }
}
