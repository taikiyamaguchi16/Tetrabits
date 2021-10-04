using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アクションインターフェース用Desc
public struct PlayerActionDesc
{
    GameObject target;
}

// アクション用インターフェース
public interface IPlayerAction
{
    void StartPlayerAction(PlayerActionDesc _desc);

    void EndPlayerAction(PlayerActionDesc _desc);
}

public class PlayerActionCtrl : MonoBehaviour
{
    [SerializeField]
    PlayerMove playerMove;

    // アクション候補リスト
    List<GameObject> candidates = new List<GameObject>();

    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    // Update is called once per frame
    void Update()
    {
        playerMove.movable = true;  // プレイヤーを行動可能に

        if (Input.GetKey("e") || XInputManager.GetButtonPress(0,XButtonType.B))  // アクションボタン
        {
            if (candidates.Count > 0 && runningAction == null)
            {
                // 一番近いオブジェクトを検索
                GameObject nearest = candidates[0];
                foreach (var can in candidates)
                {
                    if (Vector3.Distance(transform.position, can.transform.position) <
                        Vector3.Distance(transform.position, nearest.transform.position))
                        nearest = can;
                }

                // IAction持ちの一番近いやつ取得
                runningAction = nearest.GetComponent<IPlayerAction>();

                // アクション開始
                runningAction.StartPlayerAction(desc);
            }

            playerMove.movable = false; // プレイヤー行動停止
        }
        else if((Input.GetKeyUp("e") || XInputManager.GetButtonRelease(0,XButtonType.B)) && runningAction != null)   // アクションボタンリリース
        {
            // アクション終了
            runningAction.EndPlayerAction(desc);
            runningAction = null;
        }

        candidates.Clear(); // リストクリア
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            candidates.Add(other.gameObject);  // アクション候補のリストに追加
        }
    }
}
