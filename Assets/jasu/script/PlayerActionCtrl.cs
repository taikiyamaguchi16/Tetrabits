using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerActionDesc
{
    GameObject target;
}

public interface IPlayerAction
{
    void StartPlayerAction(PlayerActionDesc _desc);

    void EndPlayerAction(PlayerActionDesc _desc);
}

public class PlayerActionCtrl : MonoBehaviour
{
    // アクション候補リスト
    List<GameObject> candidates = new List<GameObject>();

    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e"))  // アクションボタン
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
        }
        else if(Input.GetKeyUp("e") && runningAction != null)   // アクションボタンリリース
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
