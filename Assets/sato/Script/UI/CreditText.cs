using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditText : MonoBehaviour
{
    [SerializeField]
    ControllerDetect controllerDetect;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // コントローラー接続時
        if (controllerDetect.GetControllerFlag())
        {
            text.text = "B で戻る";
        }
        // コントローラー非接続時
        else if (!controllerDetect.GetControllerFlag())
        {
            text.text = "ENTER で戻る";
        }
    }
}
