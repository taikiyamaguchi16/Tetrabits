using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBeltCnveyor : MonoBehaviour
{
    Vector3 conveyorDir;
    [SerializeField] float conveyorPower = 3f;
    [SerializeField] float conveyorSpeed = 3f;

    private List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        conveyorDir = transform.right;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        foreach (var rb in rigidbodies)
        {
            //物体の移動速度のベルトコンベア方向の成分だけを取り出す
            var objectSpeed = Vector3.Dot(rb.velocity, conveyorDir);

            //目標値以下なら加速する
            if (objectSpeed < Mathf.Abs(conveyorSpeed))
            {
                rb.AddForce(conveyorDir * conveyorPower);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody) rigidbodies.Add(rigidBody);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody) rigidbodies.Remove(rigidBody);
    }
}