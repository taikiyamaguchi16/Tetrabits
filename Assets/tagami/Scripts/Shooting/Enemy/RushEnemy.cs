using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RushEnemy : MonoBehaviour
{
    [SerializeField] PlayerDetector playerDeterctor;

    bool foundPlayerTrigger;

    //待機
    bool waiting;
    [SerializeField] float waitSeconds = 0.5f;
    float waitTimer;

    //突進
    [SerializeField] float rushMoveSpeed = 1.0f;
    Quaternion startRotation;
    Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //playerDirection = (collision.gameObject.transform.position - transform.position).normalized;

        //startRotation = transform.rotation;
        //endRotation = Quaternion.FromToRotation(-Vector3.right, (collision.gameObject.transform.position - transform.position).normalized);

        //見つけた
        if (!foundPlayerTrigger && playerDeterctor.foundPlayer)
        {
            foundPlayerTrigger = true;
            waiting = true;
            startRotation = transform.rotation;
            endRotation = Quaternion.FromToRotation(-Vector3.right, (playerDeterctor.foundPlayerPosition - transform.position).normalized);
        }

        //見つけてた
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitSeconds)
            {//待機完了
                waitTimer = waitSeconds;
                waiting = false;

                //その方向へベロシティをかける
                GetComponent<Rigidbody2D>().velocity = (playerDeterctor.foundPlayerPosition - transform.position).normalized * rushMoveSpeed;
            }

            var waitDt = waitTimer / waitSeconds;

            //回転
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, waitDt);
        }
    }
}
