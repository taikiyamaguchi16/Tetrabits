using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinEnemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeedX = 1.0f;
    [SerializeField] float moveSpeedY = 1.0f;
    [SerializeField] float verticalWidth = 1.0f;

    //位置
    float transformPositionX;
    float sinValue;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        transformPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        transformPositionX -= moveSpeedX * Time.deltaTime;
        sinValue += moveSpeedY * Time.deltaTime;
        transform.position = new Vector3(transformPositionX, initialPosition.y + Mathf.Sin(sinValue) * verticalWidth, initialPosition.z);


    }
}
