using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroAccelController : MonoBehaviour
{
    [SerializeField] float accelScale = 1.5f;
    [SerializeField] float accelRecast = 3.0f;

    GyroBallController ballControllerComponent;
    bool accelable = true;
    float timeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ballControllerComponent = GameObject.Find("Ball").GetComponent<GyroBallController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if(timeCount>accelRecast)
        {
            accelable = true;
            timeCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (accelable)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity *= accelScale;
            ballControllerComponent.InstanceEffect();
        }
    }
}
