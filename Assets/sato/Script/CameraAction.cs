using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraAction : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("動かしたいカメラを設定")]
    GameObject targetCamera;

    // 初期位置
    [SerializeField]
    [Header("初期位置を入力")]
    Vector3 startPos;

    // 初期位置を現在のカメラの位置で設定したい場合に使用
    [SerializeField]
    [Header("初期位置を現在のカメラポジションに自動設定")]
    [Tooltip("trueで自動、falseでマニュアル")]
    bool autoFlag;

    // 動かしたい位置
    [SerializeField]
    [Header("動かしたい位置を入力")]
    Vector3 endPos;

    // 上限人数
    [SerializeField]
    [Header("上限人数を設定")]
    int numUpperLimit = 2;

    [SerializeField]
    [Header("下限人数を設定")]
    int numLowerLimit = 2;

    // スピード
    [SerializeField]
    [Header("補間スピード")]
    float speed = 1.0f;

    // 線形補間か球形補間かを切り替える
    [SerializeField]
    [Header("線形補間か球形補間か切り替え")]
    [Tooltip("trueで線形、falseで球形")]
    bool interpolateSwitch;

    // 二点間の距離
    float distance;

    // 補正完了かどうか
    [SerializeField]
    bool isInterplate = false;

    // Start is called before the first frame update
    void Start()
    {
        // 自動設定
        if (autoFlag)
        {
            // カメラの初期位置を保存
            startPos = targetCamera.transform.position;
        }
        else
        {
            targetCamera.transform.position = startPos;
        }

        // 二点間の距離を計算
        distance = Vector3.Distance(startPos, endPos);
    }

    // Update is called once per frame
    void Update()
    {
        InterpolateCamera();
    }

    //--------------------------------------------------
    // InterpolateCamera
    // カメラ補間関数
    //--------------------------------------------------
    void InterpolateCamera()
    {
        if (isInterplate)
        {
            // 現在位置
            float presentPos = (Time.time * speed) / distance;

            // 線形補間
            if (interpolateSwitch)
            {
                // カメラの移動
                targetCamera.transform.position = Vector3.Lerp(startPos, endPos, presentPos);
            }
            // 球形補間
            else
            {
                // カメラの移動
                targetCamera.transform.position = Vector3.Slerp(startPos, endPos, presentPos);
            }

            // 設定位置まで移動完了で補間終了
            if(targetCamera.transform.position.x >= endPos.x &&
                targetCamera.transform.position.y >= endPos.y &&
                targetCamera.transform.position.z >= endPos.z)
            {
                isInterplate = false;
            }
        }
    }

    //--------------------------------------------------
    // OnPlayerEnteredRoom
    // ルームにプレイヤーが入室した際に呼ばれるコールバック
    //--------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("liauhguglaouglluirhgalhguaigli");
        // 設定した人数以上になれば
        if(PhotonNetwork.PlayerList.Length <= numUpperLimit && PhotonNetwork.PlayerList.Length >= numLowerLimit)
        {
            isInterplate = true;
        }
    }
}
