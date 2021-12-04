using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatteryLift : MonoBehaviourPunCallbacks
{
    // 第二引数をTrueにするとHDRカラーパネルになる
    [ColorUsage(false, true)] public Color color1;
    [ColorUsage(false, true)] public Color color2;

    [SerializeField]
    Transform startPos;

    [SerializeField]
    Transform endPos;

    [SerializeField]
    GameObject carryBattery;

    [SerializeField]
    float blinkingTime;
    [SerializeField]
    float blinkingInterval;
    [SerializeField]
    MeshRenderer lightMesh;

    private bool blinkFg;
    private bool nowBlinkingFg;


    private void Start()
    {
        //lightMesh.material.EnableKeyword("_EMISSION");
        blinkFg = false;
        nowBlinkingFg = false;
    }
    public void SetBatteryLiftPos(float _time, float _maxTime)
    {
        float _minus = _maxTime - blinkingTime;
        if (_time > _minus)
        {
            _time = _minus;
            if (!nowBlinkingFg)
            {
                photonView.RPC(nameof(StartBlink), RpcTarget.All);
                //StartCoroutine(StartBlink());
            }
        }
        float rate = Mathf.Clamp01(_time / _minus);   // 割合計算
        // 移動・回転
        carryBattery.transform.position = Vector3.Lerp(startPos.position, endPos.position, rate);
    }
    [PunRPC]
    IEnumerator StartBlink()
    {
        nowBlinkingFg = true;

        Coroutine _someCoroutine;
        _someCoroutine = StartCoroutine(Blink());
        yield return new WaitForSeconds(blinkingTime);
        StopCoroutine(_someCoroutine);

        blinkFg = false;
        lightMesh.material.SetColor("_EmissionColor", color2);

        nowBlinkingFg = false;
        yield break; 
    }
    IEnumerator Blink()
    {
        while (true)
        {
            if (!blinkFg)
            {
                lightMesh.material.SetColor("_EmissionColor", color1);
            }
            else
            {
                lightMesh.material.SetColor("_EmissionColor", color2);
            }
            blinkFg = !blinkFg;
            yield return new WaitForSeconds(blinkingInterval);
        }
    }

    [PunRPC]
    public void RPCStartBlink()
    {
        StartCoroutine(StartBlink());
    }
}
