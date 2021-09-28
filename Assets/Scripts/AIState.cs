using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AIState {
    public NpcControl npcControl;
    public GameObject gameObject;
    protected GameObject target;
    protected float endTime;

    public abstract void Start();
    public abstract void Update();

    protected void Move(float speed) {
        Vector3 v = gameObject.transform.forward * speed * Time.deltaTime;
        v.y = 0;

        gameObject.transform.position += v;

        Animator animator = gameObject.GetComponent<Animator>();

        if (animator != null) {
            animator.SetFloat("Speed", speed);
        }
    }

    protected void Rotate(float angle, float speed) {
        float theta = 0;
        if (angle > 0) {
            theta = speed;

            if (theta > angle) {
                theta = angle;
            }
        }
        else {
            theta = -speed;

            if (theta < angle) {
                theta = angle;
            }
        }

        gameObject.transform.Rotate(0, theta, 0);
    }

    protected bool FindTarget() {
        if (target != null && IsTargetInView(target)) {
            return true;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players) {
            if (IsTargetInView(player)) {
                target = player;
                return true;
            }
        }

        target = null;

        return false;
    }

    protected bool IsTargetInView(GameObject obj) {
        if (obj == null) {
            return false;
        }

        float distance = Vector3.Distance(obj.transform.position, gameObject.transform.position);

        return IsTargetInView(distance);
    }

    protected bool IsTargetInView(float distance) {
        if (distance > npcControl.viewDistance) {
            return false;
        }

        if (distance < 0.01f) {
            return true;
        }

        return true;
    }
}

class IdleAIState : AIState {
    public override void Start() {
        endTime = Time.time + Random.Range(1, 2);

        Animator animator = gameObject.GetComponent<Animator>();

        if (animator != null) {
            animator.SetFloat("Speed", 0);
        }
    }

    public override void Update() {
        FindTarget();

        if (Time.time >= endTime) {
            if (target == null) {
                float v = Random.value;

                if (v < 0.7f) {
                    npcControl.ChangeState("rotate");
                } else {
                    npcControl.ChangeState("walk");
                }
            } else {
                float angle = Vector3.SignedAngle(gameObject.transform.forward, target.transform.position - gameObject.transform.position, Vector3.up);

                if (Mathf.Abs(angle) > npcControl.targetAngle) {
                    npcControl.ChangeState("rotate");
                } else {
                    float v = Random.value;

                    if (v < 0.2f) {
                        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

                        if (distance > npcControl.chaseDistance) {
                            npcControl.ChangeState("chase");
                        } else {
                            npcControl.ChangeState("moveBack");
                        }
                    } else {
                        npcControl.ChangeState("attack");
                    }
                }
            }
        }
    }
}

class RotateAIState : AIState {
    public float rotateSpeed;
    private float angle;

    public RotateAIState(float rotateSpeed) {
        this.rotateSpeed = rotateSpeed;
    }

    public override void Start() {
        endTime = Time.time + Random.Range(0.5f, 1.0f);
        angle = Random.Range(-60, 60);
    }

    public override void Update() {
        FindTarget();

        if (target != null) {
            npcControl.ChangeState("watch");
            return;
        }

        Rotate(angle, rotateSpeed);

        if (Time.time >= endTime) {
            float v = Random.value;

            if (v < 0.5f) {
                npcControl.ChangeState("idle");
            } else {
                npcControl.ChangeState("walk");
            }
        }
    }
}

class WalkAIState : AIState {
    public float moveSpeed;

    public WalkAIState(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public override void Start() {
        endTime = Time.time + Random.Range(2, 4);
    }

    public override void Update() {
        FindTarget();

        if (target != null) {
            npcControl.ChangeState("watch");
            return;
        }

        Move(moveSpeed);

        if (Time.time >= endTime) {
            npcControl.ChangeState("idle");
        }
    }
}

class MoveBackAIState : AIState {
    public float moveSpeed;

    public MoveBackAIState(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public override void Start() {
        endTime = Time.time + Random.Range(1, 2);
    }

    public override void Update() {
        Move(-moveSpeed);

        if (Time.time >= endTime) {
            npcControl.ChangeState("idle");
        }
    }
}

class WatchAIState : AIState {
    public float rotateSpeed;

    public WatchAIState(float rotateSpeed) {
        this.rotateSpeed = rotateSpeed;
    }

    public override void Start() {
        endTime = Time.time + Random.Range(1, 2);

        Animator animator = gameObject.GetComponent<Animator>();

        if (animator != null) {
            animator.SetFloat("Speed", 0);
        }
    }

    public override void Update() {
        FindTarget();

        if (target != null) {
            float angle = Vector3.SignedAngle(gameObject.transform.forward, target.transform.position - gameObject.transform.position, Vector3.up);

            Rotate(angle, rotateSpeed);
        }

        if (Time.time >= endTime) {
            if (target == null) {
                npcControl.ChangeState("walk");
            } else {
                float angle = Vector3.SignedAngle(gameObject.transform.forward, target.transform.position - gameObject.transform.position, Vector3.up);

                if (Mathf.Abs(angle) > npcControl.targetAngle) {
                    npcControl.ChangeState("watch");
                } else {
                    float v = Random.value;

                    if (v < 0.2f) {
                        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

                        if (distance > npcControl.chaseDistance) {
                            npcControl.ChangeState("chase");
                        }
                        else {
                            npcControl.ChangeState("moveBack");
                        }
                    }
                    else {
                        npcControl.ChangeState("attack");
                    }
                }
            }
        }
    }
}

class ChaseAIState : AIState {
    public float moveSpeed;
    public float rotateSpeed;
    public float nearDistance;

    public ChaseAIState(float moveSpeed, float rotateSpeed, float nearDistance) {
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
        this.nearDistance = nearDistance;
    }

    public override void Start() {
        endTime = Time.time + Random.Range(1, 2);
    }

    public override void Update() {
        FindTarget();

        if (target == null) {
            npcControl.ChangeState("idle");
            return;
        }

        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

        if (distance < nearDistance) {
            npcControl.ChangeState("watch");
            return;
        }

        float angle = Vector3.SignedAngle(gameObject.transform.forward, target.transform.position - gameObject.transform.position, Vector3.up);

        Rotate(angle, rotateSpeed);
        Move(moveSpeed);

        if (Time.time >= endTime) {
            npcControl.ChangeState("watch");
        }
    }
}

class AttackAIState : AIState {
    public GameObject bullet;
    public float targetAngle;
    public float rotateSpeed;

    public AttackAIState(GameObject bullet, float targetAngle, float rotateSpeed) {
        this.bullet = bullet;
        this.targetAngle = targetAngle;
        this.rotateSpeed = rotateSpeed;
    }

    public override void Start() {
        endTime = Time.time + 1;
    }

    public override void Update() {
        FindTarget();

        if (target == null) {
            npcControl.ChangeState("watch");
            return;
        }

        float angle = Vector3.SignedAngle(gameObject.transform.forward, target.transform.position - gameObject.transform.position, Vector3.up);

        if (Mathf.Abs(angle) > targetAngle) {
            Rotate(angle, rotateSpeed);
        }

        if (Time.time >= endTime) {
            if (Mathf.Abs(angle) <= targetAngle) {
                FireBullet();
            }

            npcControl.ChangeState("watch");
        }
    }

    public void FireBullet() {
        GameObject instance = GameObject.Instantiate(bullet);
        Transform firePos = gameObject.transform.Find("FirePos");

        BulletControl bulletControl = instance.GetComponent<BulletControl>();
        bulletControl.owner = gameObject;

        instance.transform.position = firePos.position;
        instance.transform.rotation = firePos.rotation;
    }
}

