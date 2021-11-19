using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController characterController;//①CharacterController型の変数
    private Vector3 Velocity;//①キャラクターコントローラーを動かすためのVector3型の変数
    public float MoveSpeed;//①移動速度

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();//①CharacterControllerを変数に代入
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))//①Wキーがおされたら 
        {
            characterController.Move(this.gameObject.transform.forward * MoveSpeed * Time.deltaTime);//①前方にMoveSpeed＊Time.deltaTimeだけ動かす
        }

        if (Input.GetKey(KeyCode.S))//①Sキーがおされたら
        {
            characterController.Move(this.gameObject.transform.forward * -1f * MoveSpeed * Time.deltaTime);//①後方にMoveSpeed＊Time.deltaTimeだけ動かす
        }

        if (Input.GetKey(KeyCode.A))//①Aキーがおされたら 
        {
            characterController.Move(this.gameObject.transform.right * -1 * MoveSpeed * Time.deltaTime);//①左にMoveSpeed＊Time.deltaTimeだけ動かす
        }

        if (Input.GetKey(KeyCode.D))//①Dキーがおされたら 
        {
            characterController.Move(this.gameObject.transform.right * MoveSpeed * Time.deltaTime);//①右にMoveSpeed＊Time.deltaTimeだけ動かす
        }

        characterController.Move(Velocity);//①キャラクターコントローラーをVelocityだけ動かし続ける
        Velocity.y += Physics.gravity.y * Time.deltaTime;//①Velocityのy軸を重力*Time.deltaTime分だけ動かす

    }
}