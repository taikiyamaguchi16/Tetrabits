using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectGenerate : MonoBehaviourPunCallbacks
{
    // 生成するオブジェクトを指8定
    [SerializeField, Tooltip("スポーンさせるオブジェクトを設定")]
    GameObject obj;

    // 生成数カウント用
    int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjCreate();
    }

    //--------------------------------------------------
    // ObjCreate
    // キー入力でオブジェクトを生成
    //--------------------------------------------------
    void ObjCreate()
    {
        // エンターキーで生成
        if (Input.GetKey(KeyCode.Return))
        {
            GameObject spawned = PhotonNetwork.Instantiate(obj.name, Vector3.zero, Quaternion.identity);
            cnt++;
            Debug.Log("生成数：" + cnt);
        }
    }
}
