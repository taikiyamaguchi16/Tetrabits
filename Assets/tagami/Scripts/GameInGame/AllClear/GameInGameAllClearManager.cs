using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICreditActivate
{
    void OnActivated();
}

public class GameInGameAllClearManager : MonoBehaviour
{
    //[System.Serializable]
    //class CreditInput
    //{
    //    public string post;
    //    public string name;
    //}
    //[Header("Credit")]
    //[SerializeField] Transform parentCanvas;
    //[SerializeField] GameObject creditNamePrefab;
    //[SerializeField] GameObject creditPostPrefab;
    //Generic.UIEasing currentUIEasing;
    //Generic.UIEasing subCurrentUIEasing;

    //[Header("Credit Inputs")]
    //[SerializeField] List<CreditInput> creditInputs;
    //int creditInputIndex = 0;

    bool oldPowerdOn;

    [Header("Credit Scroll")]
    [SerializeField] float scrollSpeed = 1.0f;
    [SerializeField] float fastForwardScrollSpeed = 5.0f;

    [SerializeField] GameObject scrollParent;
    bool stopedScroll;
    [SerializeField] List<RawImage> fastForwardImages;
    [SerializeField] Color fastForwardColor;
    [SerializeField] Color fastForwardDefaultColor;

    [Header("Credit End")]
    [SerializeField] RawImage fadeImage;
    [SerializeField] Text toTitleText;

    [Header("Background")]
    [SerializeField] RawImage frontBackground;
    [SerializeField] RawImage backBackground;
    float backgroundFadeSeconds;
    float backgroundFadeTimer;
    bool backgroundFading;


    // Start is called before the first frame update
    void Start()
    {
        VirtualCameraManager.OnlyActive(1);
        //StartCoroutine(CoCreateCredit());

        //初期カラー設定
        foreach (var fastForwardImage in fastForwardImages)
            fastForwardImage.color = fastForwardDefaultColor;
    }

    IEnumerator CoEndCredit()
    {
        yield return new WaitForSeconds(5);

        //フェードアウト
        while (true)
        {
            var color = fadeImage.color;
            color.a += Time.deltaTime;
            fadeImage.color = color;
            if (color.a >= 1.0f)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        //終了準備
        toTitleText.enabled = true;
        //終了ループ
        while (true)
        {
            VirtualCameraManager.OnlyActive(0);
            if ((XInputManager.GetButtonTrigger(0, XButtonType.A) || Input.GetKeyDown(KeyCode.Return)))
            {
                GameInGameUtil.DisconnectAndReloadGameMain();
            }
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //スクロール速度
        if (!stopedScroll)
        {

            if (TetraInput.sTetraLever.GetPoweredOn())
            {
                scrollParent.transform.localPosition += -Vector3.right * fastForwardScrollSpeed * Time.deltaTime;
            }
            else
            {
                scrollParent.transform.localPosition += -Vector3.right * scrollSpeed * Time.deltaTime;
            }

            if (TetraInput.sTetraLever.GetPoweredOn() && !oldPowerdOn)
            {
                //GameObject.Find("Display")?.GetComponent<CRTNoise>()?.

                foreach (var fastForwardImage in fastForwardImages)
                    fastForwardImage.color = fastForwardColor;
            }
            else if (!TetraInput.sTetraLever.GetPoweredOn() && oldPowerdOn)
            {
                foreach (var fastForwardImage in fastForwardImages)
                    fastForwardImage.color = fastForwardDefaultColor;
            }

            oldPowerdOn = TetraInput.sTetraLever.GetPoweredOn();
        }

        //タイトルへ
        bool xinputConnected = false;
        for (int i = 0; i < 4; i++)
        {
            if (XInputManager.IsConnected(i))
            {
                xinputConnected = true;
                break;
            }
        }
        if (xinputConnected)
        {
            toTitleText.text = "Aでタイトルに戻る";
        }
        else
        {
            toTitleText.text = "Enterでタイトルに戻る";
        }

        //バックグラウンド
        if (backgroundFading)
        {
            backgroundFadeTimer += Time.deltaTime;
            if (backgroundFadeTimer >= backgroundFadeSeconds)
            {
                backgroundFadeTimer = backgroundFadeSeconds;
                backgroundFading = false;
            }
            var frontColor = frontBackground.color;
            frontColor.a = 1 - (backgroundFadeTimer / backgroundFadeSeconds);
            frontBackground.color = frontColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        ICreditActivate iactivate;
        if (collision.TryGetComponent(out iactivate))
        {
            iactivate.OnActivated();
        }
    }

    public void ChangeFadeBackground(Texture _nextTexture, float _fadeSeconds)
    {
        frontBackground.texture = backBackground.texture;
        backBackground.texture = _nextTexture;
        backgroundFadeSeconds = _fadeSeconds;
        backgroundFadeTimer = 0.0f;
        backgroundFading = true;
    }

    public void EndCreditScroll()
    {
        stopedScroll = true;
        StartCoroutine(CoEndCredit());
    }
}
