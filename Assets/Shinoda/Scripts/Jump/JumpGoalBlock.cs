using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGoalBlock : MonoBehaviour
{
    [SerializeField] GameObject JumpUI;
    JumpTimeController JumpTimeControllerComponent;

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