using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroBallController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip bounceSE;
    [SerializeField] AudioClip leverSE;

    [SerializeField] float rotateScale = 0.1f;
    [SerializeField] float stopTime = 1.0f;
    [SerializeField] float gravityScale = 9.8f;

    [SerializeField] GameObject effectPrefab = null;
    [SerializeField] Transform effectInstanceTransform;
    float timeCount;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(BGM, 1.0f);
        GameInGameUtil.StartGameInGameTimer("gyro");
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();
        timeCount += Time.deltaTime;

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
        }
        if (leverState ^ TetraInput.sTetraLever.GetPoweredOn()) SimpleAudioManager.PlayOneShot(leverSE);
        leverState = TetraInput.sTetraLever.GetPoweredOn();
        //if (TetraInput.sTetraButton.GetTrigger())
        //{
        //    rb.velocity = Vector2.zero;
        //}



        rb.AddForce(-(this.transform.up) * gravityScale);

        effectInstanceTransform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);

        float speed = rb.velocity.magnitude;
        Debug.Log(speed);
        if (speed < 10)
        {
            if (timeCount > 5)
            {
                InstanceEffect();
                timeCount = 0;
            }
        }
        else if (speed > 10 && speed < 30)
        {
            if (timeCount > 2.5f)
            {
                InstanceEffect();
                timeCount = 0;
            }
        }
        else if (speed > 30)
        {
            if (timeCount > 0.5f)
            {
                InstanceEffect();
                timeCount = 0;
            }
        }
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