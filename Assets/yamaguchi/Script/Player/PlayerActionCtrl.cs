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

    bool GetIsActionPossible(PlayerActionDesc _desc);

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
    //高優先度のアクション候補
    List<GameObject> allActionItem = new List<GameObject> { };
    //同一優先度の場合に近いほうを実行するためのリスト
    List<GameObject> highPriorityList = new List<GameObject>();


    //自身の情報を送る(仮)
    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    //アクション実行予定のオブジェクト
    private GameObject selectedObj;

    [SerializeField]
    private Animator playerAnim;

    private void Awake()
    {
        desc.playerObj = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {           
            if (Input.GetKeyDown("e") || XInputManager.GetButtonTrigger(playerMove.controllerID, XButtonType.B))  // アクションボタン
            {       
                //持ち運んでいるオブジェクトがある場合それをアクション候補に加える
                GameObject carryObj = holder.GetItem();
                if (carryObj != null)
                {
                    if (!allActionItem.Contains(carryObj))
                    {
                        allActionItem.Add(carryObj);
                    }
                }

                if (allActionItem.Count > 0)
                    CheckItemPossible();
                if (candidates.Count > 0)
                { 
                    //優先度が高いアクションを判別して実行
                    PriorityCheck();
                    CheckHighPriorityAction();

                    runningAction = selectedObj.GetComponent<IPlayerAction>();
                    runningAction.StartPlayerAction(desc);

                    playerMove.SetPlayerMovable(false); // プレイヤー行動停止

                    SetActionAnim();
                }               

                allActionItem.Remove(carryObj);
            }
            else if (Input.GetKeyUp("e") || XInputManager.GetButtonRelease(playerMove.controllerID, XButtonType.B))   // アクションボタンリリース
            {
                playerMove.SetPlayerMovable(true);  // プレイヤーを行動可能に
                if (runningAction != null)
                {
                    // アクション終了
                    runningAction.EndPlayerAction(desc);
                    runningAction = null;
                    candidates.Clear();
                    highPriorityList.Clear();
                }
            }   

        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            if (!allActionItem.Contains(other.gameObject))
            {
                allActionItem.Add(other.gameObject);  // アクション候補のリストに追加

                CheckItemPossible();
                if (candidates.Count > 0)
                {
                    //優先度が高いアクションを判別
                    PriorityCheck();
                    CheckHighPriorityAction();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            allActionItem.Remove(other.gameObject);  // アクション候補のリスト

            if (allActionItem.Count > 0)
            {
                CheckItemPossible();
                if (candidates.Count > 0)
                {
                    PriorityCheck();
                    CheckHighPriorityAction();
                }
            }
        }
    }
    private void PriorityCheck()
    {
        highPriorityList.Clear();
        //毎回ゲットコンポーネントしないようにlist作成
        List<IPlayerAction> intList = new List<IPlayerAction>();       
        foreach (var can in candidates)
        {     
            intList.Add(can.GetComponent<IPlayerAction>());         
        }
       
        IPlayerAction max = intList[0];

        highPriorityList.Add(candidates[intList.IndexOf(max)]);
        if (intList.Count > 1) 
        {
            for (int i = 1; i < intList.Count; i++)
            {
                if (intList[i].GetPriority() > max.GetPriority())
                {
                    //最大優先度が変更になっているのでリストを消去
                    highPriorityList.Clear();
                    //優先度の高いアクションを格納
                    max = intList[i];
                    highPriorityList.Add(candidates[intList.IndexOf(max)]);  
                }
                else if(intList[i].GetPriority() == max.GetPriority())
                {
                    highPriorityList.Add(candidates[intList.IndexOf(max)]);
                }
            }  
        }      
    }
    private void CheckHighPriorityAction()
    {
        if (highPriorityList.Count > 0)
        {
            // 一番近いオブジェクトを検索
            GameObject nearest = highPriorityList[0];
            foreach (var can in highPriorityList)
            {
                if (Vector3.Distance(transform.position, can.transform.position) <
                    Vector3.Distance(transform.position, nearest.transform.position))
                    nearest = can;
            }
            //IAction持ちの一番近いやつ取得
            selectedObj = nearest;
        }
    }
    private void CheckItemPossible()
    {
        GameObject deleteObj = null;
        candidates.Clear();
        foreach (var item in allActionItem)
        {
            if (item != null)
            {
                if (item.GetComponent<IPlayerAction>().GetIsActionPossible(desc))
                {
                    if (!candidates.Contains(item))
                        candidates.Add(item);

                }
            }
            else
            {
                deleteObj = item;
            }
        }
        allActionItem.Remove(deleteObj);
    }

    private void SetActionAnim()
    {
       // playerAnim.SetBool("Walking", false);
        playerAnim.SetTrigger("Actioning");
        //playerAnim.SetBool("Waiting", false);
    }
}
