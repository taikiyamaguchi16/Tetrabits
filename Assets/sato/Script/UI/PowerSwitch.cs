using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    // 電源判定
    bool isPower = false;

    [SerializeField]
    GameObject TitleLogo;

    [SerializeField]
    [Header("コントーローラーID")]
    public int controllerID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        // PadのBもしくはキーボードのBで起動
        if (XInputManager.GetButtonTrigger(controllerID, XButtonType.B) || Input.GetKeyDown(KeyCode.B))
        {
            isPower = true;
        }
    }
}
