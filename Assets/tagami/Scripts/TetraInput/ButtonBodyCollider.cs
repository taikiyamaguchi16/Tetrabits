using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBodyCollider : MonoBehaviour
{
    public int collisionNum { private set; get; } = 0;

    private void OnCollisionEnter(Collision collision)
    {
        collisionNum++;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionNum--;
    }
}
