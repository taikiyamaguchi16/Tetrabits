using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// アクションインターフェース用Desc
public struct PlayerActionDesc
{
   public GameObject target;
}

// アクション用インターフェース
public interface IPlayerAction
{
    void StartPlayerAction(PlayerActionDesc _desc);

    void EndPlayerAction(PlayerActionDesc _desc);
}

public class PlayerActionCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    PlayerMove playerMove;

    // アクション候補リスト
    List<GameObject> candidates = new List<GameObject>();

    //何かしら保持しているか
    //private bool isOwn;
    private Coal ownObj;

    //自身の情報を送る(仮)
    PlayerActionDesc desc;
    //持ち替えた際は捨てられない
    public bool cantDump;

    // 実行中アクション
    IPlayerAction runningAction = null;

    private void Start()
    {
        desc.target = this.gameObject;
        cantDump = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            playerMove.movable = true;  // プレイヤーを行動可能に

            if (Input.GetKey("e") || XInputManager.GetButtonPress(playerMove.controllerID, XButtonType.B))  // アクションボタン
            {
                if (ownObj != null)
                    candidates.Remove(ownObj.gameObject);
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
                //保有しているオブジェクトがあったら捨てる
                if (ownObj != null && !cantDump)
                {
                    ownObj.Dump();
                    cantDump = true;
                    ownObj = null;
                }
                playerMove.movable = false; // プレイヤー行動停止
            }
            else if (Input.GetKeyUp("e") || XInputManager.GetButtonRelease(playerMove.controllerID, XButtonType.B))   // アクションボタンリリース
            {
                
                cantDump = false;
                if (runningAction != null)
                {
                    // アクション終了
                    runningAction.EndPlayerAction(desc);
                    runningAction = null;
                }
            }

            candidates.Clear(); // リストクリア
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            candidates.Add(other.gameObject);  // アクション候補のリストに追加
        }
    }

    public void ChangeHolding(Coal _sc)
    {
        if (ownObj != null)
        {
            ownObj.Dump();
        }
        ownObj = _sc;
        cantDump = true;
    }
}
