using System.Collections;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class PlayerStatus : StrixBehaviour {
    [StrixSyncField]
    public int health = 100;
    public int maxHealth = 100;
    public float recoverTime = 3;
    private Animator animator;
    private float deadTime = 0;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!isLocal) {
            return;
        }

        if (health <= 0 && Time.time >= deadTime + recoverTime) {
            RpcToAll("SetHealth", maxHealth);
        }
    }

    [StrixRpc]
    public void OnHit() {
        int value = health - 10;

        if (value < 0) {
            value = 0;
        }

        RpcToAll("SetHealth", value);
    }

    [StrixRpc]
    public void SetHealth(int value) {
        if (value < 0) {
            value = 0;
        } else if (value > maxHealth) {
            value = maxHealth;
        }

        animator.SetInteger("Health", value);

        if (animator != null) {
            if (value < health) {
                if (value <= 0) {
                    animator.SetTrigger("Dead");
                } else {
                    animator.SetTrigger("Damaged");
                }
            }

            if (value > 0 && health <= 0) {
                Respawn();
            }
        }

        if(health != value) {
            if (value <= 0) {
                deadTime = Time.time;
            }
        }

        health = value;
    }

    private void Respawn() {
        if (!CompareTag("Player") && isLocal) {
            transform.position = new Vector3(Random.Range(-40.0f, 40.0f), 2, Random.Range(-40.0f, 40.0f));
        }
    }
}

