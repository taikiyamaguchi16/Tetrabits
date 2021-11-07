using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
// アクションインターフェース用Desc
public struct PlayerActionDesc
{
   public GameObject playerObj;
}

// アクション用インターフェース
public interface IPlayerAction
{
    void StartPlayerAction(PlayerActionDesc _desc);

    void EndPlayerAction(PlayerActionDesc _desc);

    int GetPriority();
}

public class PlayerActionCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    PlayerMove playerMove;

    [SerializeField]
    ItemPocket holder;
    // アクション候補リスト
    List<GameObject> candidates = new List<GameObject>();
    
    //自身の情報を送る(仮)
    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    private void Awake()
    {
        desc.playerObj = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            playerMove.movable = true;  // プレイヤーを行動可能に

            if (Input.GetKeyDown("e") || XInputManager.GetButtonPress(playerMove.controllerID, XButtonType.B))  // アクションボタン
            {
                //持ち運んでいるオブジェクトがある場合それをアクション候補に加える
                GameObject carryObj = holder.GetItem();
                if (carryObj != null)
                    if (!candidates.Contains(carryObj))
                    {
                        candidates.Add(carryObj);
                        
                    }
                if (candidates.Count > 0 && runningAction == null)
                {
                    PriorityCheck();
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
                    Debug.Log(nearest.name+"　のアクションを実行");
                    // アクション開始
                    runningAction.StartPlayerAction(desc);         
                }
                playerMove.movable = false; // プレイヤー行動停止
            }
            else if (Input.GetKeyUp("e") || XInputManager.GetButtonRelease(playerMove.controllerID, XButtonType.B))   // アクションボタンリリース
            {
                if (runningAction != null)
                {
                    // アクション終了
                    runningAction.EndPlayerAction(desc);
                    runningAction = null;
                    //candidates.Clear();
                }
            }   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            if (!candidates.Contains(other.gameObject))
            {
                candidates.Add(other.gameObject);  // アクション候補のリストに追加            
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            candidates.Remove(other.gameObject);  // アクション候補のリスト             
        }
    }
    private void PriorityCheck()
    {
        //毎回ゲットコンポーネントしないようにlist作成
        List<IPlayerAction> intList = new List<IPlayerAction> { };
        //一旦オブジェクトを指定してから消す
        List<GameObject> keepList = new List<GameObject> { };
        //Debug.Log(candidates.Count + "数は");
        foreach (var can in candidates)
        {
            intList.Add(can.GetComponent<IPlayerAction>());
        }
       
        IPlayerAction max = intList[0];
        for (int i = 1; i < intList.Count; i++)
        {
            //if (intList[i].GetPriority() == max.GetPriority())
            //    Debug.Log("同じ");
           　if (intList[i].GetPriority() < max.GetPriority())
            {
                //アクション候補から優先度の低いものを排除                 
                keepList.Add(candidates[i]);
            }
            else if (intList[i].GetPriority() > max.GetPriority())
            {
                //アクション候補から優先度の低いものを排除            
                keepList.Add(candidates[intList.IndexOf(max)]);
                max = intList[i];
            }
        }

        foreach (var kp in keepList)
        {
            candidates.Remove(kp);
        }
    }


}
