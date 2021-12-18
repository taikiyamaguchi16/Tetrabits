using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroBallController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer myRenderer;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip bounceSE;
    [SerializeField] AudioClip leverSE;

    [SerializeField] float rotateScale = 0.1f;
    [SerializeField] float stopTime = 1.0f;
    [SerializeField] float gravityScale = 9.8f;
    [SerializeField] Color stopColor;

    [SerializeField] GameObject effectPrefab = null;
    [SerializeField] Transform effectInstanceTransform;
    GameObject accelEffect = null;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(BGM, 1.0f);
        GameInGameUtil.StartGameInGameTimer("gyro");
        rb = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        accelEffect = Instantiate(effectPrefab, effectInstanceTransform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();

        if (padVec.x > 0)
        {
            this.transform.Rotate(0.0f, 0.0f, 1.0f * rotateScale);
        }
        else if (padVec.x < 0)
        {
            this.transform.Rotate(0.0f, 0.0f, -1.0f * rotateScale);
        }

        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            rb.velocity = Vector2.zero;
            myRenderer.color = stopColor;
        }
        else myRenderer.color = Color.white;
        if (leverState ^ TetraInput.sTetraLever.GetPoweredOn()) SimpleAudioManager.PlayOneShot(leverSE);
        leverState = TetraInput.sTetraLever.GetPoweredOn();
        //if (TetraInput.sTetraButton.GetTrigger())
        //{
        //    rb.velocity = Vector2.zero;
        //}

        rb.AddForce(-(this.transform.up) * gravityScale);

        effectInstanceTransform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);

        float speed = rb.velocity.magnitude;
    }

    public void InstanceEffect()
    {
        GameObject accelEffect = Instantiate(effectPrefab, effectInstanceTransform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SimpleAudioManager.PlayOneShot(bounceSE);
    }
}