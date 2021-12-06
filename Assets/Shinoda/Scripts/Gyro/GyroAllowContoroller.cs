using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroAllowContoroller : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject goal;
    [SerializeField] float distance = 5f;
    Vector3 workVec;

    // Start is called before the first frame update
    void Start()
    {
        if (goal == null) goal = GameObject.Find("Goal");
        //workVec = (goal.transform.position - player.transform.position).normalized;
        //this.transform.position = (player.transform.position) + (workVec * distance);
    }

    // Update is called once per frame
    void Update()
    {
        workVec = (goal.transform.position - player.transform.position).normalized;
        this.transform.position = (player.transform.position) + (workVec * distance);
        this.transform.rotation = Quaternion.FromToRotation(Vector3.up, workVec);
    }
}