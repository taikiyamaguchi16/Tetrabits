using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// プレーヤー生成時に行う初期設定処理
/// </summary>
public class PlayerInitializeSetting : MonoBehaviourPunCallbacks
{
    //private const int _PLAYER_UPPER_LIMIT = 4;

    ///// <summary>
    ///// プレイヤーに番号を与える
    ///// </summary>
    //private void SetMyCustomProperties()
    //{
    //    //自分のクライアントの同期オブジェクトにのみ
    //    if (photonView.IsMine)
    //    {
    //        List<int> playerSetableCountList = new List<int>();

    //        //制限人数までの数字のリストを作成
    //        //例) 制限人数 = 4 の場合、{0,1,2,3}
    //        int count = 0;
    //        for (int i = 0; i < _PLAYER_UPPER_LIMIT; i++)
    //        {
    //            playerSetableCountList.Add(count);
    //            count++;
    //        }

    //        //他の全プレイヤー取得
    //        Player[] otherPlayers = PhotonNetwork.PlayerListOthers;

    //        //他のプレイヤーがいなければカスタムプロパティの値を"0"に設定
    //        if (otherPlayers.Length <= 0)
    //        {
    //            //ローカルのプレイヤーのカスタムプロパティを設定
    //            int playerAssignNum = otherPlayers.Length;
    //            PhotonNetwork.LocalPlayer.UpdatePlayerNum(playerAssignNum);
    //            return;
    //        }

    //        //他のプレイヤーのカスタムプロパティー取得してリスト作成
    //        List<int> playerAssignNums = new List<int>();
    //        for (int i = 0; i < otherPlayers.Length; i++)
    //        {
    //            playerAssignNums.Add(otherPlayers[i].GetPlayerNum());
    //        }

    //        //リスト同士を比較し、未使用の数字のリストを作成
    //        //例) 0,1にプレーヤーが存在する場合、返すリストは2,3
    //        playerSetableCountList.RemoveAll(playerAssignNums.Contains);

    //        //ローカルのプレイヤーのカスタムプロパティを設定
    //        //空いている場所のうち、一番若い数字の箇所を利用
    //        PhotonNetwork.LocalPlayer.UpdatePlayerNum(playerSetableCountList[0]);
    //    }
    //}

    private void Start()
    {
        //プレーヤーのカスタムプロパティ更新
        //SetMyCustomProperties();
    }

    /// <summary>
    /// カスタムプロパティ更新時のコールバック
    /// </summary>
    /// <param name="target">更新されたカスタムプロパティを持つプレーヤー</param>
    /// <param name="changedProps">更新されたカスタムプロパティ</param>
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        ////自分のクライアントの同期オブジェクトの設定
        //if (photonView.IsMine)
        //{
        //    this.gameObject.transform.rotation = _playerInitTransform[PhotonNetwork.LocalPlayer.GetPlayerNum()].rotation;
        //    this.gameObject.transform.position = _playerInitTransform[PhotonNetwork.LocalPlayer.GetPlayerNum()].position;
        //    _avatarObjectMeshRenderer.sharedMaterial = _playerMaterials[PhotonNetwork.LocalPlayer.GetPlayerNum()];
        //}
        ////他のクライアントの同期オブジェクトの設定
        //else
        //{
        //    this.gameObject.transform.rotation = _playerInitTransform[photonView.Owner.GetPlayerNum()].rotation;
        //    this.gameObject.transform.position = _playerInitTransform[photonView.Owner.GetPlayerNum()].position;
        //    _avatarObjectMeshRenderer.sharedMaterial = _playerMaterials[photonView.Owner.GetPlayerNum()];
        //}
    }
}
