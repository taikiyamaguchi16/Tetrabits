using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
public class CreditPlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 100.0f;

    [Header("Bomb")]
    [SerializeField] GameObject bombFieldPrefab;

    Rigidbody2D myRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("動かねぇ");

        //移動
        myRigidbody2D.velocity = TetraInput.sTetraPad.GetVector() * moveSpeed * Time.deltaTime;

        if (TetraInput.sTetraButton.GetTrigger())
        {
            Instantiate(bombFieldPrefab, transform);
        }

        //position制限
        var localPosition = transform.localPosition;
        if (localPosition.x >= 910.0f)
        {
            localPosition.x = 910.0f;
        }
        else if (localPosition.x <= -910.0f)
        {
            localPosition.x = -910.0f;
        }
        if (localPosition.y >= 490.0f)
        {
            localPosition.y = 490.0f;
        }
        else if (localPosition.y <= -490.0f)
        {
            localPosition.y = -490.0f;
        }
        transform.localPosition = localPosition;
    }
}
