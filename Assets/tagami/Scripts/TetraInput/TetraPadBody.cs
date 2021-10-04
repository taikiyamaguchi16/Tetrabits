using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPadBody : MonoBehaviour
{
    public List<GameObject> padOnList { private set; get; }

    private void Awake()
    {
        padOnList = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        padOnList.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        padOnList.Remove(collision.gameObject);
    }
}
