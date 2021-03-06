using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GyroBallController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer myRenderer;
    PhotonTransformViewClassic photonTransformView;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip bounceSE;
    [SerializeField] AudioClip leverSE;

    [SerializeField] float rotateScale = 0.1f;
    [SerializeField] float stopTime = 1.0f;
    [SerializeField] float gravityScale = 9.8f;
    [SerializeField] Color moveColor;
    [SerializeField] Color stopColor;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(BGM, 1.0f);
        GameInGameUtil.StartGameInGameTimer("gyro");
        rb = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        photonTransformView = GetComponent<PhotonTransformViewClassic>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 padVec = TetraInput.sTetraPad.GetVector();

        //if (padVec.x > 0)
        //{
        //    this.transform.Rotate(0.0f, 0.0f, padVec.magnitude * rotateScale);
        //}
        //else if (padVec.x < 0)
        //{
        //    this.transform.Rotate(0.0f, 0.0f, -padVec.magnitude * rotateScale);
        //}

        //if (TetraInput.sTetraLever.GetPoweredOn())
        //{
        //    rb.velocity = Vector2.zero;
        //    myRenderer.color = stopColor;
        //}
        //else myRenderer.color = moveColor;
        //if (leverState ^ TetraInput.sTetraLever.GetPoweredOn()) SimpleAudioManager.PlayOneShot(leverSE);
        //leverState = TetraInput.sTetraLever.GetPoweredOn();

        //rb.AddForce(-(this.transform.up) * gravityScale);

        //photonTransformView.SetSynchronizedValues(speed: rb.velocity, turnSpeed: 0);
    }

    private void FixedUpdate()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();

        if (padVec.x > 0)
        {
            this.transform.Rotate(0.0f, 0.0f, padVec.magnitude * rotateScale);
        }
        else if (padVec.x < 0)
        {
            this.transform.Rotate(0.0f, 0.0f, -padVec.magnitude * rotateScale);
        }

        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            rb.velocity = Vector2.zero;
            myRenderer.color = stopColor;
        }
        else myRenderer.color = moveColor;
        if (leverState ^ TetraInput.sTetraLever.GetPoweredOn()) SimpleAudioManager.PlayOneShot(leverSE);
        leverState = TetraInput.sTetraLever.GetPoweredOn();

        rb.AddForce(-(this.transform.up) * gravityScale);

        photonTransformView.SetSynchronizedValues(speed: rb.velocity, turnSpeed: 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SimpleAudioManager.PlayOneShot(bounceSE);
    }
}