using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditActivatedEnd : MonoBehaviour,ICreditActivate
{
    [SerializeField] GameInGameAllClearManager allClearManager;
    [SerializeField] UnityEngine.UI.RawImage startedDisableRawImage;

    // Start is called before the first frame update
    void Start()
    {
        startedDisableRawImage.enabled = false;

        if(!allClearManager)
        {
            Debug.LogError("allClearManagerを設定してください");
        }
    }

    public void OnActivated()
    {
        Debug.Log("終了通知を送信");
        allClearManager.EndCreditScroll();
    }
}
