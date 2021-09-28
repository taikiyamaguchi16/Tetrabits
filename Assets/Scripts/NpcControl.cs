using System;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcControl : StrixBehaviour {
    public GameObject bullet;
    public float walkSpeed = 1;
    public float forwardSpeed = 4;
    public float backwardSpeed = 2;
    public float rotateSpeed = 2;
    public float viewDistance = 15;
    public float targetAngle = 10;
    public float nearDistance = 3;
    public float chaseDistance = 7;
    private AIState aiState;
    private Dictionary<string, AIState> stateCache = new Dictionary<string, AIState>();
    private PlayerStatus playerStatus;

    // Use this for initialization
    void Start() {
        playerStatus = GetComponent<PlayerStatus>();

        AddState("idle", new IdleAIState());
        AddState("rotate", new RotateAIState(rotateSpeed));
        AddState("walk", new WalkAIState(walkSpeed));
        AddState("moveBack", new MoveBackAIState(backwardSpeed));
        AddState("watch", new WatchAIState(rotateSpeed));
        AddState("chase", new ChaseAIState(forwardSpeed, rotateSpeed, nearDistance));
        AddState("attack", new AttackAIState(bullet, 3, rotateSpeed));

        ChangeState("idle");
    }

    // Update is called once per frame
    void Update() {
        if (!isLocal) {
            return;
        }

        if (playerStatus != null && playerStatus.health <= 0) {
            return;
        }

        aiState.Update();
    }

    public void ChangeState(string name) {
        //Debug.Log("ChangeState " + name);

        aiState = GetState(name);
        aiState.gameObject = gameObject;
        aiState.npcControl = this;
        aiState.Start();
    }

    public void AddState(string name, AIState state) {
        stateCache[name] = state;
    }

    public void RemoveState(string name) {
        stateCache.Remove(name);
    }

    public AIState GetState(string stateName) {
        return stateCache[stateName];
    }
}
