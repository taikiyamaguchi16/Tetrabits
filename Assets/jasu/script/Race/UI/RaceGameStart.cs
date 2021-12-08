using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaceGameStart : MonoBehaviourPunCallbacks
{
    [SerializeField]
    SceneObject firstStageScene = null;

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger() && firstStageScene != null)
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    photonView.RPC(nameof(RPCStartGameTimer), RpcTarget.All);
            //}
            GameInGameUtil.StartGameInGameTimer("race");
            GameInGameUtil.SwitchGameInGameScene(firstStageScene);
        }
    }

    //[PunRPC]
    //private void RPCStartGameTimer()
    //{
    //    GameInGameUtil.StartGameInGameTimer("race");
    //}
}
