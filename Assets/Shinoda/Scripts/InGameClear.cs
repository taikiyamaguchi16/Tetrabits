using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameClear : MonoBehaviour
{
    Transform textTransform;
    bool animEnd = false;
    bool usePopper = false;
    bool loadable = true;

    [SerializeField] string gameTag;
    [SerializeField] float switchTime = 3f;
    float timeCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        textTransform = transform.Find("Clear");
        TextAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if(animEnd&&!usePopper)
        {
            // クラッカー鳴らす
            usePopper = true;
        }

        if(usePopper)
        {
            timeCount += Time.deltaTime;
            if (timeCount > switchTime && loadable)
            {
                GameInGameUtil.StopGameInGameTimer(gameTag);
                GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                loadable = false;
            }
        }
    }

    void TextAnimation()
    {
        textTransform.DOScale(1.0f, 1.0f).SetEase(Ease.InQuint).OnComplete(() =>
         {
             animEnd = true;
         });
    }
}
