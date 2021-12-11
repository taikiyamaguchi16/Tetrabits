using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpGoalBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject JumpUI;
    JumpTimeController JumpTimeControllerComponent;

    bool isGoal = false;

    // Start is called before the first frame update
    void Start()
    {
        if (JumpUI == null) JumpUI = GameObject.Find("JumpUI");
        JumpTimeControllerComponent = JumpUI.GetComponent<JumpTimeController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        JumpTimeControllerComponent.GoalAnimation();
    }
}