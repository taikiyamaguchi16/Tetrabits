using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShooter : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject bulletPrefab;

    [Header("Status")]
    [SerializeField] Vector3 shotOffset;
    [SerializeField] float bulletSpeed = 5.0f;
    [SerializeField] string targetName = "Jet";
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > shotIntervalSeconds)
        {
            shotTimer = 0.0f;

            //ターゲット選択
            var targetObj = GameObject.Find(targetName);
            if (targetObj)
            {
                var bulletObj = Instantiate(
                    bulletPrefab,
                    transform.position + shotOffset,
                    //Quaternion.FromToRotation(Vector3.forward, (targetObj.transform.position - transform.position).normalized)
                    Quaternion.identity
                    );

                bulletObj.GetComponent<Rigidbody2D>().velocity = (targetObj.transform.position - transform.position).normalized * bulletSpeed;
                GameInGameUtil.MoveGameObjectToOwnerScene(bulletObj, gameObject);
            }
            else
            {
                Debug.LogWarning("Targetが見つからないので弾を生成できません");
            }
        }
    }
}
