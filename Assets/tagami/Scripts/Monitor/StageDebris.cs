using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StageDebris : MonoBehaviourPunCallbacks, ICool
{
    [Header("Status")]
    [SerializeField] GameObject bodyObject;
    [SerializeField] float hpMax = 1.0f;
    float hp;

    [Header("Fall")]
    [SerializeField] float fallOffsetY = 10.0f;
    [SerializeField] float fallSeconds = 1.0f;
    float fallTimer;
    Vector3 bodyEndLocalPosition;

    [Header("Option")]
    [SerializeField] UnityEngine.UI.Slider debugSlider;

    // Start is called before the first frame update
    void Start()
    {
        bodyEndLocalPosition = bodyObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //落とす
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSeconds)
        {
            fallTimer = fallSeconds;
        }

        bodyObject.transform.localPosition = Vector3.Lerp(Vector3.up * fallOffsetY, bodyEndLocalPosition, fallTimer / fallSeconds);


        if (debugSlider)
        {
            debugSlider.value = hp / hpMax;
        }
    }

    public bool GetActiveSelf()
    {
        return bodyObject.activeSelf;
    }

    public void CallSetActive(bool _active)
    {
        photonView.RPC(nameof(RPCSetActive), RpcTarget.All, _active);
    }
    [PunRPC]
    public void RPCSetActive(bool _active)
    {
        bodyObject.SetActive(_active);

        GetComponent<Collider>().enabled = _active;

        if (_active)
        {
            //HPをまんたんにする
            hp = hpMax;

            //timerをスタートする
            fallTimer = 0.0f;
        }
    }

    public void OnCooled(float _damage)
    {
        hp -= _damage;
        if (hp <= 0 && PhotonNetwork.IsMasterClient)
        {
            CallSetActive(false);
        }
    }
}
