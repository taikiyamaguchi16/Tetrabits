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

    //自身の情報を送る(仮)
    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    [SerializeField]
    private Animator playerAnim;

    private void Awake()
    {
        desc.playerObj = this.gameObject;
        //playerAnim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            foreach(var ttt in allActionItem)
            {
                Debug.Log(ttt.name);
            }
        }
        if (photonView.IsMine)
        {
            playerMove.movable = true;  // プレイヤーを行動可能に

            if (Input.GetKeyDown("e") || XInputManager.GetButtonPress(playerMove.controllerID, XButtonType.B))  // アクションボタン
            {
                
                //持ち運んでいるオブジェクトがある場合それをアクション候補に加える
                GameObject carryObj = holder.GetItem();
                if (carryObj != null)
                {          
                    if (!allActionItem.Contains(carryObj))
                        allActionItem.Add(carryObj);
                }

                if (allActionItem.Count > 0)
                    CheckItemPossible();
                if (candidates.Count > 0 && runningAction == null)
                    PriorityCheck();


                playerMove.movable = false; // プレイヤー行動停止

                allActionItem.Remove(carryObj);
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
            if (!allActionItem.Contains(other.gameObject))
            {
                allActionItem.Add(other.gameObject);  // アクション候補のリストに追加            
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            allActionItem.Remove(other.gameObject);  // アクション候補のリスト             
        }
    }
    private void PriorityCheck()
    {       
        //毎回ゲットコンポーネントしないようにlist作成
        List<IPlayerAction> intList = new List<IPlayerAction> { };
        foreach (var can in candidates)
        {     
            intList.Add(can.GetComponent<IPlayerAction>());
        }
       
        IPlayerAction max = intList[0];
        
        if (intList.Count > 1) 
        {
            for (int i = 1; i < intList.Count; i++)
            {
                if (intList[i].GetPriority() < max.GetPriority())
                {
                    //アクション候補から優先度の低いものを排除          
                    //highPriorityList.Add(candidates[i]);
                }
                else if (intList[i].GetPriority() > max.GetPriority())
                {
                    //アクション候補から優先度の低いものを排除            
                    //highPriorityList.Add(candidates[intList.IndexOf(max)]);
                    max = intList[i];
                }
            }  
        }
        runningAction = max;
        //一旦ここで実行させる
        runningAction.StartPlayerAction(desc);

        candidates.Clear();

        PhotonNetwork.Destroy
    }
    private void PlayHighPriorityAction()
    {
        if (candidates.Count > 0)
        {
            // 一番近いオブジェクトを検索
            GameObject nearest = candidates[0];
            foreach (var can in candidates)
            {
                Debug.Log("アクション候補  " + can.name);
                if (Vector3.Distance(transform.position, can.transform.position) <
                    Vector3.Distance(transform.position, nearest.transform.position))
                    nearest = can;
            }
            //IAction持ちの一番近いやつ取得

           runningAction = nearest.GetComponent<IPlayerAction>();
           runningAction.StartPlayerAction(desc);
            //アクション開始
        }
    }

    private void CheckItemPossible()
    {    
        foreach(var item in allActionItem)
        {
            if (item.GetComponent<IPlayerAction>().GetIsActionPossible(desc))
            {
                if (!candidates.Contains(item))
                {
                    candidates.Add(item);
                }
            }
        }
    }
}
