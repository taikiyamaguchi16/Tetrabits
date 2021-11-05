using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DOManager : MonoBehaviour
{
    [SerializeField]
    [Header("イージングタイプ指定")]
    protected Ease easeTypes;

    [SerializeField]
    [Header("ループタイプ指定")]
    protected LoopType loopTypes;

    [SerializeField]
    [Header("ループさせたい回数(-1で無限ループ)")]
    protected int loopTimes;

    [Header("・移動")]

    [SerializeField]
    [Header("各挙動と変化量を設定(併用可)")]
    protected bool moveFlag = false;
    [SerializeField]
    [Tooltip("移動先の位置")]
    protected Vector3 moveRange = Vector3.zero;
    [SerializeField]
    [Tooltip("何秒かけて移動させるか")]
    protected float moveTime = 0.0f;

    [SerializeField]
    [Header("・スケール")]
    protected bool scaleFlag = false;
    [SerializeField]
    [Tooltip("変化させたい目的値")]
    protected float scaleRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    protected float scaleTime = 0.0f;

    [SerializeField]
    [Header("・回転")]
    protected bool rotateFlag = false;
    [SerializeField]
    [Tooltip("回転軸指定")]
    protected Vector3 rotateAxis = Vector3.up;
    [SerializeField]
    [Tooltip("回転量指定")]
    protected float rotateRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    protected float rotateTime = 0.0f;

    [SerializeField]
    [Header("・カラー")]
    protected bool ColorFlag = false;
    [SerializeField]
    [Tooltip("Rendererがあれば入れる")]
    protected Renderer rendererComponent;
    [SerializeField]
    [Tooltip("imageがあれば入れる")]
    protected Image image;
    [SerializeField]
    [Tooltip("指定した色に変化")]
    protected Color color = Color.red;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    protected float colorTime = 0.0f;

    [SerializeField]
    [Header("・フェード(アルファ値、音量も可)")]
    protected bool FadeFlag = false;
    [SerializeField, Range(0f, 1f)]
    [Tooltip("変化させたい目的値(0～1)")]
    protected float fadeRange = 0.0f;
    [SerializeField]
    [Tooltip("何秒かけて変化させるか")]
    protected float fadeTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
