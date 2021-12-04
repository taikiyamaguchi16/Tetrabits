using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トリガー内のオブジェクトの数を数える
public class TriggerSensor : MonoBehaviour
{
    //int inTriggerNum;   // オブジェクトのカウント

    //bool existInTrigger = false; // トリガー内に何かしらのオブジェクトがあればtrue;

    //private void Start()
    //{
    //    inTriggerNum = 0;
    //}

    //private void Update()
    //{
    //    if(inTriggerNum > 0)
    //    {
    //        existInTrigger = true;
    //    }
    //    else
    //    {
    //        existInTrigger = false;
    //        inTriggerNum = 0;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    inTriggerNum++;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    inTriggerNum--;
    //}

    //public int GetInTriggerNum()
    //{
    //    return inTriggerNum;
    //}

    //public bool GetExistInTrigger()
    //{
    //    return existInTrigger;
    //}

    private bool Grounded;
    public float Jumppower;

    private Ray ray; // 飛ばすレイ
    private float distance = 0.5f; // レイを飛ばす距離
    private RaycastHit hit; // レイが何かに当たった時の情報
 
    [SerializeField]
    Transform rayPosition;
    [SerializeField]
    PlayerMove playerMove;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(rayPosition.position, transform.up * -1); // レイを下に飛ばす
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red); // レイを赤色で表示させる

        if (Physics.Raycast(ray, out hit, distance)) // レイが当たった時の処理
        {
            playerMove.SetPlayerJumpable(true);
        }
        else
        {
            playerMove.SetPlayerJumpable(false);
        }
    }
}
