using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitOnlinePlayer : MonoBehaviourPunCallbacks
{
    private const int _PLAYER_UPPER_LIMIT = 4;

    [SerializeField]
    GameObject Own;

    [SerializeField]
    Text nameText;

    [SerializeField]
    GameObject textReady;

    int controllerID = 0;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties["isPlayerReady"] is bool isPlayerReady)
            {
                if (isPlayerReady)
                {
                    photonView.RPC(nameof(WaitPlayerSyncThings), RpcTarget.All);
                }
            }
            photonView.RPC(nameof(WaitPlayerSyncName), RpcTarget.All);
        }
    }

    [PunRPC]
    public void WaitPlayerSyncThings()
    {
        textReady.SetActive(true);

        anim.SetBool("isKeirei", true);
    }

    [PunRPC]
    public void WaitPlayerSyncName()
    {
        if (gameObject.name == "Keirei_Yellow(Clone)")
        {
            nameText.color = Color.yellow;
        }

        if (gameObject.name == "Keirei_Red(Clone)")
        {
            nameText.color = Color.red;
        }

        if (gameObject.name == "Keirei_Green(Clone)")
        {
            nameText.color = Color.green;
        }

        if (gameObject.name == "Keirei_Blue(Clone)")
        {
            nameText.color = Color.blue;
        }

        nameText.text = photonView.Owner.NickName;
    }
}
