using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
public class TouchedPadEffect : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] float lifeSeconds = 1.0f;
    float lifeTimer;

    [Header("Scale")]
    Vector3 startLocalScale = Vector3.one;
    [SerializeField] Vector3 endLocalScale = Vector3.one;

    //SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();

        startLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //生存時間
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeSeconds)
        {
            lifeTimer = lifeSeconds;
            Destroy(gameObject);
        }

        var dt = lifeTimer / lifeSeconds;


        //Scale変更
        transform.localScale = Vector3.Lerp(startLocalScale, endLocalScale, dt);

        //アルファ値変更
        //var color = spriteRenderer.color;
        //color.a = 1 - dt;
        //spriteRenderer.color = color;
    }
}
