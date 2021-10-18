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
    [SerializeField] bool completeInactiveUnitTest;

    // Start is called before the first frame update
    void Awake()
    {
        //ユニットテストの機能を切りたい場合
        if(completeInactiveUnitTest)
        {
            occupancy = true;   //占有状態にしておく
        }

        
        if (occupancy)
        {//占有状態の場合、UnitTest用の機能を削除

            //初期状態でUnitTestオブジェクトは起動しておきたいため
            //このタイミングで削除する
            foreach (var obj in UnitTestObjects)
            {
                obj.SetActive(false);
            }

            //カメラをモニター用に設定
            //SetCanvassWorldCamera(monitorCamera);
        }
        else
        {
            //UnitTest用の機能を起動

            //ユニットテスト用機能を起動
            //SetCanvassWorldCamera(unitTestCamera);
        }
    }

    private void Update()
    {
        //Debug.Log("w:"+Screen.width +"h:"+ Screen.height);
    }

    //void SetCanvassWorldCamera(Camera _camera)
    //{
    //    foreach (var canvas in canvass)
    //    {
    //        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    //        canvas.worldCamera = _camera;
    //    }
    //}
}
