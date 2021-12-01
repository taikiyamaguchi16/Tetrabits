using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitInsertCassetteManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.Find("CassetteHolder");
        if (obj)
        {
            obj.GetComponent<CassetteManager>().AppearAllCassette();
        }
        else
        {
            Debug.LogError("カセットを出現させる処理に失敗しました");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
