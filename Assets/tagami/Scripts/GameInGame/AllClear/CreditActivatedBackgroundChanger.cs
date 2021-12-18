using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditActivatedBackgroundChanger : MonoBehaviour, ICreditActivate
{
    [Header("Status")]
    [SerializeField] Texture background;

    [Header("Required Manager")]
    [SerializeField] GameInGameAllClearManager allClearManager;
    
    [Header("Prefab Reference")]
    [SerializeField] UnityEngine.UI.RawImage startedDisebleRawImage;

    // Start is called before the first frame update
    void Start()
    {
        startedDisebleRawImage.enabled = false;

        if (!allClearManager)
        {
            Debug.LogError("allClearManagerが設定されていないと背景を変更できません");
        }
    }

    public void OnActivated()
    {
        allClearManager.ChangeFadeBackground(background, 2);
    }
}
