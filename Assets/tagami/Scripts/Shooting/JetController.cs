using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject bulletPrefab;

    [Header("Status")]
    [SerializeField] float moveSpeed = 1.0f;

    [SerializeField] float shotIntervalTime = 1.0f;
    float shotTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetPress())
        {
            Debug.Log("Bomb!");
            foreach(var obj in GameObject.FindGameObjectsWithTag("Bullet"))
            {
                Destroy(obj);
            }

        }


        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            shotTimer += Time.deltaTime;
            if (shotTimer >= shotIntervalTime)
            {
                shotTimer = 0.0f;
                //発射
                var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().velocity = Vector2.right*10.0f;
            }
        }
        else
        {
            shotTimer = 0.0f;
        }

        //Move
        Vector3 moveVec = Vector3.zero;
        moveVec.x += TetraInput.sTetraPad.GetVector().x;
        moveVec.y += TetraInput.sTetraPad.GetVector().y;
        transform.position += moveVec * moveSpeed * Time.deltaTime;
    }
}
