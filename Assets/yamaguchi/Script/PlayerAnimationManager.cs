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

    private Dictionary<string,Texture> playerTextures = new Dictionary<string,Texture>();

    [SerializeField]
    OverrideSprite overrideSprite;

    void Start()
    {

        animatorStateEvent = AnimatorStateEvent.Get(animator, 0);

        // ステートが変わった時のコールバック
        animatorStateEvent.stateEntered += _ => ChangeTexture();

        if (photonView.IsMine)
        {
            myNumber = PhotonNetwork.CountOfPlayers;
            photonView.RPC(nameof(RPCSetOthersTexture), RpcTarget.All, photonView.ViewID, myNumber);
        }
        else
        {

        } 
    }


    public void ChangeTexture()
    {    
        overrideSprite.SetTexture(playerTextures[animatorStateEvent.CurrentStateName]);
    }

    [PunRPC]
    public void RPCSetOthersTexture(int _id,int _number)
    {
        if(photonView.ViewID==_id)
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
}