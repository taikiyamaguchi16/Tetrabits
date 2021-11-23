using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class NumBeat : DOManager
{
    GameObject parent;

    bool isSelect = false;

    Vector3 InitScale;

    // Start is called before the first frame update
    void Start()
    {
        // このスクリプトをアタッチしているオブジェクトの親を取得
        parent = gameObject.transform.parent.gameObject;

        // 初期サイズ保存
        InitScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        ParentSelected();
    }

    // 親が選択されているか取得
    void ParentSelected()
    {
        if (!isSelect)
        {
            // 親が選択されている
            if (EventSystem.current.currentSelectedGameObject == parent)
            {
                isSelect = true;
                transform.DOScale(scaleRange, scaleTime).SetEase(easeTypes).SetLoops(loopTimes, loopTypes);
            }
        }

        // 親が選択されていない
        if(EventSystem.current.currentSelectedGameObject != parent)
        {
            isSelect = false;
            transform.DOScale(InitScale, 0.1f);
        }
        
        //else
        //{
        //    transform.DOScale(Vector3.zero, 0);
        //}

        //try
        //{

        //}
        //catch (NullReferenceException ex)
        //{

        //}
    }
}
