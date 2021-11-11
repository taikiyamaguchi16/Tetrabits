using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCamera : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;

    bool stopping;

    // Update is called once per frame
    void Update()
    {
        if (!stopping)
        {
            transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    public void StopCamera()
    {
        stopping = true;
    }

    public void RestartCamera()
    {
        stopping = false;
    }
}
