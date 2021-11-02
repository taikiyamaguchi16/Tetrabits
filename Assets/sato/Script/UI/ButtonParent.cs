using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonParent : DOManager
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallMove()
    {
        // 現在位置から設定した位置に移動
        transform.DOLocalMove(moveRange, moveTime).SetRelative(true);
    }
}
