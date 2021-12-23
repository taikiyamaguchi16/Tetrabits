using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnemyDirtAttack : MonoBehaviour
{
    bool isAttacking = false;

    [SerializeField]
    SpriteBlink spriteBlink;

    [SerializeField]
    GameObject dirtSplashPrefab;

    [SerializeField]
    float warningTimeSeconds = 3f;

    float warningTimer = 0f;

    [SerializeField]
    float dirtInterval = 0.1f;

    float dirtTimer = 0f;

    [SerializeField]
    int dirtNum = 25;

    int dirtCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteBlink.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            warningTimer += Time.deltaTime;
            if(warningTimer > warningTimeSeconds)
            {
                spriteBlink.active = false;

                dirtTimer += Time.deltaTime;
                if(dirtTimer > dirtInterval)
                {
                    dirtTimer = 0f;

                    GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
                    GameInGameUtil.MoveGameObjectToOwnerScene(dirtSplashObj, gameObject);
                    dirtSplashObj.transform.parent = transform;
                    Vector3 pos = transform.position;
                    pos.y = 50f;
                    pos.z -= 3f * dirtCounter;
                    dirtSplashObj.transform.position = pos;
                    dirtCounter++;
                    if(dirtCounter >= dirtNum)
                    {
                        warningTimer = 0f;
                        dirtTimer = 0f;
                        dirtCounter = 0;
                        isAttacking = false;
                    }
                }
            }
        }
    }

    public void AttackStart()
    {
        if (!isAttacking)
        {
            warningTimer = 0f;
            dirtTimer = 0f;
            dirtCounter = 0;
            isAttacking = true;
            spriteBlink.active = true;
        }
    }
}
