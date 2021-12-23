using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceBackScroll : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    RacerController racerController;

    [SerializeField]
    float spd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (racerController.GetRigidbody().velocity.z > 1f)
        {
            spriteRenderer.material.SetFloat("_ScrollX", -spd);
        }
        else
        {
            spriteRenderer.material.SetFloat("_ScrollX", 0);
        }
    }
}
