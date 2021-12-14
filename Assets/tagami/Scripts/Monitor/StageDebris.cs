using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StageDebris : MonoBehaviourPunCallbacks
{
    [Header("Status")]
    [SerializeField] GameObject bodyObject;
    [SerializeField] float hpMax = 1.0f;
    float hp;

    [Header("Fall")]
    [SerializeField] float fallOffsetY = 10.0f;
    [SerializeField] float fallSeconds = 1.0f;
    Vector3 bodyEndLocalPosition;

    [Header("FallingUI")]
    [SerializeField] float flashSeconds = 5.0f;
    [SerializeField] SpriteRenderer fallingPositionUIRenderer;
    [SerializeField] AnimationCurve fallingPositionUIAlphaCurve;

    [Header("Sound")]
    [SerializeField] SEAudioClip landClip;

    [Header("Effect")]
    [SerializeField] GameObject fireEffect;
    Vector3 fireEffectScaleMax;
    [SerializeField, Range(0, 1)] float fireEffectScaleMinMultiplier = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //記録しておく
        bodyEndLocalPosition = bodyObject.transform.localPosition;
        fireEffectScaleMax = fireEffect.transform.localScale;

        //bodyとUIをオフに
        bodyObject.SetActive(false);
        var color = fallingPositionUIRenderer.color;
        color.a = 0;
        fallingPositionUIRenderer.color = color;
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
        //GetComponent<Collider>().enabled = _active;

        if (_active)
        {
            StartCoroutine(CoActivate());
        }
    }

    IEnumerator CoActivate()
    {
        //HPをまんたんにする
        hp = hpMax;

        //炎最大サイズ
        fireEffect.transform.localScale = Vector3.Lerp(fireEffectScaleMax * fireEffectScaleMinMultiplier, fireEffectScaleMax, 1);
        //上にあげとく
        bodyObject.transform.localPosition = Vector3.Lerp(Vector3.up * fallOffsetY, bodyEndLocalPosition, 0);

        //UI点滅
        float uiTimer = 0.0f;
        while (true)
        {
            uiTimer += Time.deltaTime;
            if (uiTimer >= flashSeconds)
            {
                uiTimer = flashSeconds;
            }

            var color = fallingPositionUIRenderer.color;
            color.a = fallingPositionUIAlphaCurve.Evaluate(uiTimer / flashSeconds);
            fallingPositionUIRenderer.color = color;

            if (uiTimer >= flashSeconds)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        //落とす
        var fallTimer = 0.0f;
        //Timer更新
        while (true)
        {
            fallTimer += Time.deltaTime;
            if (fallTimer >= fallSeconds)
            {
                fallTimer = fallSeconds;
            }
            bodyObject.transform.localPosition = Vector3.Lerp(Vector3.up * fallOffsetY, bodyEndLocalPosition, fallTimer / fallSeconds);

            //抜ける
            if (fallTimer >= fallSeconds)
            {
                SimpleAudioManager.PlayOneShot(landClip);
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnCooled(float _damage)
    {
        hp -= _damage;
        if (hp <= 0 && PhotonNetwork.IsMasterClient)
        {
            CallSetActive(false);
        }

        //炎の大きさ更新
        fireEffect.transform.localScale = Vector3.Lerp(fireEffectScaleMax * fireEffectScaleMinMultiplier, fireEffectScaleMax, hp / hpMax);
    }
}
