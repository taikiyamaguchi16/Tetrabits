using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimationManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Animator animator;

    private AnimatorStateEvent animatorStateEvent;

    int myNumber = 0;

    private Dictionary<string, Texture> playerTextures = new Dictionary<string, Texture>();

    [SerializeField]
    OverrideSprite overrideSprite;

    private const int _PLAYER_UPPER_LIMIT = 4;

    /// <summary>
    /// プレイヤーに番号を与える
    /// </summary>
    private void SetMyCustomProperties()
    {
        //自分のクライアントの同期オブジェクトにのみ
        if (photonView.IsMine)
        {
            List<int> playerSetableCountList = new List<int>();

            //制限人数までの数字のリストを作成
            //例) 制限人数 = 4 の場合、{0,1,2,3}
            int count = 0;
            for (int i = 0; i < _PLAYER_UPPER_LIMIT; i++)
            {
                playerSetableCountList.Add(count);
                count++;
            }

            //他の全プレイヤー取得
            Player[] otherPlayers = PhotonNetwork.PlayerListOthers;

            //他のプレイヤーがいなければカスタムプロパティの値を"0"に設定
            if (otherPlayers.Length <= 0)
            {
                //ローカルのプレイヤーのカスタムプロパティを設定
                int playerAssignNum = otherPlayers.Length;
                PhotonNetwork.LocalPlayer.UpdatePlayerNum(playerAssignNum);
                return;
            }

            //他のプレイヤーのカスタムプロパティー取得してリスト作成
            List<int> playerAssignNums = new List<int>();
            for (int i = 0; i < otherPlayers.Length; i++)
            {
                playerAssignNums.Add(otherPlayers[i].GetPlayerNum());
            }

            //リスト同士を比較し、未使用の数字のリストを作成
            //例) 0,1にプレーヤーが存在する場合、返すリストは2,3
            playerSetableCountList.RemoveAll(playerAssignNums.Contains);

            //ローカルのプレイヤーのカスタムプロパティを設定
            //空いている場所のうち、一番若い数字の箇所を利用
            PhotonNetwork.LocalPlayer.UpdatePlayerNum(playerSetableCountList[0]);
        }
    }

    void Start()
    {
        animatorStateEvent = AnimatorStateEvent.Get(animator, 0);
        // ステートが変わった時のコールバック
        animatorStateEvent.stateEntered += _ => ChangeTexture();
        SetMyCustomProperties();
    }


    public void ChangeTexture()
    {
        if(playerTextures.Count>0)
            overrideSprite.SetTexture(playerTextures[animatorStateEvent.CurrentStateName]);
    }

    [PunRPC]
    public void RPCSetOthersTexture(int _id, int _number)
    {
        if (photonView.ViewID == _id)
        {
            Texture[] textures = null;
            switch (_number)
            {
                case 1:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerY");
                    break;
                case 2:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerR");
                    break;
                case 3:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerG");
                    break;
                case 4:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerB");
                    break;
            }
            foreach (var tex in textures)
            {
                playerTextures.Add(tex.name, tex);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        //自分のクライアントの同期オブジェクトの設定
        if (photonView.IsMine)
        {
            Texture[] textures = null;
            switch (PhotonNetwork.LocalPlayer.GetPlayerNum())
            {
                case 0:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerY");
                    break;
                case 1:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerR");
                    break;
                case 2:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerG");
                    break;
                case 3:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerB");
                    break;
            }
            foreach (var tex in textures)
            {
                if (!playerTextures.ContainsKey(tex.name))
                {
                    playerTextures.Add(tex.name, tex);
                }
            }
            overrideSprite.SetTexture(playerTextures["Idol_front"]);

        }
        //他のクライアントの同期オブジェクトの設定
        else
        {
            Texture[] textures = null;
            switch (photonView.Owner.GetPlayerNum())
            {
                case 0:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerY");
                    break;
                case 1:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerR");
                    break;
                case 2:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerG");
                    break;
                case 3:
                    textures = Resources.LoadAll<Texture>("Textures/PlayerB");
                    break;
            }
            foreach (var tex in textures)
            {
                if (!playerTextures.ContainsKey(tex.name))
                {
                    playerTextures.Add(tex.name, tex);
                }
            }
            overrideSprite.SetTexture(playerTextures["Idol_front"]);
        }
    }
}