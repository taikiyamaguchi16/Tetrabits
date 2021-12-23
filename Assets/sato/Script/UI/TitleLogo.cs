using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleLogo : DOManager
{
    [SerializeField]
    GameObject buttonParent;

    Button[] selectButton;

    TitleDemoManager DemoManager;

    [SerializeField]
    GameObject bgm;

    [SerializeField]
    Sprite[] titleLogos;

    // ロゴの切り替えフラグ
    bool logoSwitcher = false;

    int titleCount = 0;

    float time = 0.0f;

    [SerializeField]
    [Header("ロゴの切り替え秒数指定")]
    float TimerLimit = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DemoManager = GameObject.Find("DemoManager").GetComponent<TitleDemoManager>();

        // アタッチされたキャンバスの子のボタンコンポーネント取得
        selectButton = buttonParent.GetComponentsInChildren<Button>();

        Scaling();
    }

    // Update is called once per frame
    void Update()
    {
        LogoSwitching();
    }

    void Scaling()
    {
        if (scaleFlag)
        {
            // スケールを指定したアニメーションをさせながら変更
            transform.DOScale(scaleRange, scaleTime).SetEase(easeTypes).SetLoops(loopTimes, loopTypes).OnComplete(() =>
            {
                for(int i = 0; i < selectButton.Length; i++)
                {
                    selectButton[i].interactable = true;
                }
                buttonParent.GetComponent<ButtonParent>().CallMove();

                DemoManager.isStopInstantiateSwitcher(true);
                bgm.SetActive(true);

                logoSwitcher = true;
            });
        }
    }

    void LogoSwitching()
    {
        if (logoSwitcher)
        {
            switch (titleCount)
            {
                case 0:
                    gameObject.GetComponent<Image>().sprite = titleLogos[titleCount];
                    break;

                case 1:
                    gameObject.GetComponent<Image>().sprite = titleLogos[titleCount];
                    break;

                case 2:
                    gameObject.GetComponent<Image>().sprite = titleLogos[titleCount];
                    break;

                case 3:
                    gameObject.GetComponent<Image>().sprite = titleLogos[titleCount];
                    break;

                default:
                    // 最初から
                    titleCount = 0;
                    break;
            }

            time += Time.deltaTime;

            if(time >= TimerLimit)
            {
                time = 0.0f;
                titleCount++;
            }
        }
    }
}
