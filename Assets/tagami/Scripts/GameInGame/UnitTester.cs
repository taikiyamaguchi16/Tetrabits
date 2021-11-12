using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTester : MonoBehaviour
{
    static bool occupancy = false;

    [Header("Unit Reference")]
    [SerializeField] List<GameObject> UnitTestObjects;

    //この機能はUIにRenderTextureを貼り付けるだけの機能に置き換えられました
    //[Header("Canvas")]
    //[SerializeField] Camera monitorCamera;
    //[SerializeField] Camera unitTestCamera;
    //[SerializeField] List<Canvas> canvass;

    [Header("Option")]
    [SerializeField] bool completeDestroyUnitTest;

    // Start is called before the first frame update
    void Awake()
    {
        //ユニットテストの機能を切りたい場合
        if(completeDestroyUnitTest)
        {
            occupancy = true;   //占有状態にしておく
        }
        
        if (occupancy)
        {//占有状態の場合、UnitTest用の機能を削除

            //初期状態でUnitTestオブジェクトは起動しておきたいため
            //このタイミングで削除する
            foreach (var obj in UnitTestObjects)
            {
                Destroy(obj);
            }
        }
        else
        {//UnitTest用の機能を起動

        }
    }
}
