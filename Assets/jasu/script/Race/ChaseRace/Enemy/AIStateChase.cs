using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateChase : AIState
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    PlayerClashSensor playerClashSensor;

    [SerializeField]
    ChaseRaceManager raceManager;

    [SerializeField]
    RacerController racerController;

    [SerializeField]
    public int handleRacerSpdGear = 1;

    float[] racerSpdGears;

    [SerializeField]
    float warpDistance = 80f;

    [SerializeField]
    Vector3 warpOffset;

    float moveSpd;

    bool roarable = true;

    [SerializeField]
    AudioClip roarSe;

    [SerializeField]
    float roarVol = 2f;

    float roarInterval = 10f;

    float roarTimer = 0f;

    [Header("泥かけ攻撃")]

    [SerializeField]
    RaceEnemyDirtAttack[] dirtAtks;

    [SerializeField]
    float attackInterval = 10f;

    float attackTimer = 5f;

    [SerializeField, Range(0, 100)]
    float atkPercent = 50f;

    [Header("デバッグ")]

    [SerializeField]
    Vector3 velocity;

    [SerializeField]
    float distance;

    [SerializeField]
    bool goaled;

    private void Start()
    {
        racerSpdGears = racerController.GetRacerMove().GetMoveSpdGears();
    }

    private void Update()
    {
        velocity = rb.velocity;

        if (raceManager.goaled)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public override void StateUpdate()
    {
        if (!roarable)
            roarTimer += Time.deltaTime;

        // 一定以上離れたとき
        distance = Mathf.Abs(racerController.transform.position.z - transform.position.z);

        if (distance > warpDistance)
        {
            moveSpd = racerSpdGears[handleRacerSpdGear] * 2f;
            if (roarable)
            {
                SimpleAudioManager.PlayOneShot(roarSe, roarVol);
                roarable = false;
            }
        }
        else
        {
            moveSpd = racerSpdGears[handleRacerSpdGear];

            if(roarTimer > roarInterval)
            {
                roarTimer = 0f;
                roarable = true;
            }
        }

        // 攻撃
        attackTimer += Time.deltaTime;
        if(attackTimer > attackInterval)
        {
            attackTimer = 0f;
            
            if(Random.Range(0, 100) < atkPercent)
            {
                SimpleAudioManager.PlayOneShot(roarSe, roarVol);
                dirtAtks[Random.Range(0, dirtAtks.Length)].AttackStart();
            }
        }
    }

    public override void StateFixedUpdate()
    {
        // 移動
        if (!playerClashSensor.playerClash)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveSpd);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        }
    }
}
