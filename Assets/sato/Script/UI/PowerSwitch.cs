using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerSwitch : MonoBehaviour
{
    // 電源判定
    bool isPower = false;

    [SerializeField]
    GameObject TitleLogo;

    [SerializeField]
    ControllerDetect controllerDetect;

    TextMeshProUGUI tmp;

    [SerializeField]
    [Header("コントーローラーID")]
    public int controllerID = 0;

    [SerializeField]
    [Header("決定音")]
    AudioClip decisionSe;

    [SerializeField]
    float Volume = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        PowerOn();

        if (isPower)
        {
            TitleLogo.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // 起動
    void PowerOn()
    {
        // コントローラー接続時
        if (controllerDetect.GetControllerFlag())
        {
            tmp.text = "B   電源 ON";
        }
        // コントローラー非接続時
        else if (!controllerDetect.GetControllerFlag())
        {
            tmp.text = "SPACE or ENTER   電源 ON";
        }

        // メニュー表示インプット
        if (XInputManager.GetButtonTrigger(controllerID, XButtonType.B) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            isPower = true;

            SimpleAudioManager.PlayOneShot(decisionSe, Volume);
        }
    }
}
