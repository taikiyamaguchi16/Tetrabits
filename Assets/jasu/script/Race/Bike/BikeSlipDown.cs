using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BikeSlipDown : MonoBehaviourPunCallbacks
{
    public bool isSliping { get; private set; }

    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    OverrideSprite overrideSprite = null;

    Sprite defaultSprite;

    [SerializeField]
    Sprite slipSprite = null;

    [SerializeField]
    float slipingTimeSeconds = 2f;

    [SerializeField]
    float defaultRotAngle = 360f;

    float rotAngle;

    Vector3 eulerWhenSlipStart;
    
    float variation;

    float rotCounter;

    // Start is called before the first frame update
    void Start()
    {
        rotAngle = defaultRotAngle;

        defaultSprite = spriteRenderer.sprite;

        variation = rotAngle / slipingTimeSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSliping)
        {
            transform.Rotate(0, 0, variation * Time.deltaTime);
            rotCounter += variation * Time.deltaTime;
            if (rotCounter >= rotAngle)
            {
                if(PhotonNetwork.IsMasterClient)
                    photonView.RPC(nameof(RPCSlipEnd), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void RPCSlipStart()
    {
        if (isSliping)
            return;

        isSliping = true;
        eulerWhenSlipStart = new Vector3(0, transform.localEulerAngles.y, 0);
        rotCounter = 0f;

        if (animator != null)
            animator.enabled = false;

        if (overrideSprite != null)
            overrideSprite.enabled = false;

        spriteRenderer.sprite = slipSprite;
    }

    [PunRPC]
    private void RPCSlipEnd()
    {
        transform.localEulerAngles = eulerWhenSlipStart;
        isSliping = false;

        if (animator != null)
            animator.enabled = true;

        if (overrideSprite != null)
            overrideSprite.enabled = true;

        spriteRenderer.sprite = defaultSprite;
    }

    public void CallSlipStart()
    {
        rotAngle = defaultRotAngle;
        variation = rotAngle / slipingTimeSeconds;
        photonView.RPC(nameof(RPCSlipStart), RpcTarget.All);
    }

    public void CallSlipStart(string _damage)
    {
        rotAngle = defaultRotAngle;
        variation = rotAngle / slipingTimeSeconds;
        if (PhotonNetwork.IsMasterClient)
        {
            MonitorManager.DealDamageToMonitor(_damage);
            photonView.RPC(nameof(RPCSlipStart), RpcTarget.All);
        }
    }

    public void CallSlipStart(string _damage, float _seconds, float _rotAngle)
    {
        rotAngle = _rotAngle;
        variation = _rotAngle / _seconds;
        if (PhotonNetwork.IsMasterClient)
        {
            MonitorManager.DealDamageToMonitor(_damage);
            photonView.RPC(nameof(RPCSlipStart), RpcTarget.All);
        }
    }
}
