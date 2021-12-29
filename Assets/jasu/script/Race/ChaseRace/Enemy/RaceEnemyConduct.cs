using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEnemyConduct : MonoBehaviour
{
    [SerializeField]
    float conductWidth = 10f;

    [SerializeField]
    float rate = 0.01f;

    Vector3 defaultPos;

    bool inverse = true;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = defaultPos;
        if (inverse)
        {
            targetPos.x += conductWidth;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, rate);
            if(Vector3.Distance(transform.localPosition, targetPos) < 0.5f)
            {
                inverse = false;
            }
        }
        else
        {
            targetPos.x -= conductWidth;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, rate);
            if (Vector3.Distance(transform.localPosition, targetPos) < 0.5f)
            {
                inverse = true;
            }
        }
    }
}
