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
public struct CheckPriorityDesc
{
    private int pri;
    public GameObject ob;
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
    BatteryHolder holder;
    // アクション候補リスト
    List<GameObject> candidates = new List<GameObject>();
    
    //自身の情報を送る(仮)
    PlayerActionDesc desc;

    // 実行中アクション
    IPlayerAction runningAction = null;

    private void Start()
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
                GameObject carryObj = holder.GetBattery();
                if (carryObj != null)
                    candidates.Add(carryObj);

                if (candidates.Count > 0 && runningAction == null)
                {
                    //PriorityCheck();
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
            else if (Input.GetKeyUp("e") || XInputManager.GetButtonRelease(playerMove.controllerID, XButtonType.B))   // アクションボタンリリース
            {
                if (runningAction != null)
                {
                    // アクション終了
                    runningAction.EndPlayerAction(desc);
                    runningAction = null;           
                }
            }   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //candidates.Clear();
        if (other.gameObject.GetComponent<IPlayerAction>() != null)
        {
            candidates.Add(other.gameObject);  // アクション候補のリストに追加            
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
        Debug.Log(candidates.Count);
        IPlayerAction max = intList[0];
        for (int i = 1; i < intList.Count; i++)
        {
            if (intList[i].GetPriority() < max.GetPriority())
            {
                //アクション候補から優先度の低いものを排除
                Debug.Log(intList.IndexOf(intList[i]) + "を排除");
                candidates.RemoveAt(intList.IndexOf(intList[i]));
            }
            else if (intList[i].GetPriority() > max.GetPriority())
            {
                //アクション候補から優先度の低いものを排除
                candidates.RemoveAt(intList.IndexOf(max));
                max = intList[i];
            }
        }  
    }
}
