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

    [SerializeField]
    GameObject DemoManager;

    // Start is called before the first frame update
    void Start()
    {
        // アタッチされたキャンバスの子のボタンコンポーネント取得
        selectButton = buttonParent.GetComponentsInChildren<Button>();

        Scaling();
    }

    // Update is called once per frame
    void Update()
    {

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

                DemoManager.SetActive(true);
            });
        }
    }
}
