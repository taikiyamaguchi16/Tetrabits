using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
public class CreditPlayerController : MonoBehaviour
{
    [SerializeField] float impulseForce = 10.0f;
    [SerializeField] Texture pressedbuttonTexture;
    [SerializeField] Texture releasedbuttonTexture;
    RawImage buttonImage;

    Rigidbody2D myRigidbody2D;

    bool buttonTrigger;
    bool usedButtonTrigger;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<RawImage>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        myRigidbody2D.velocity = TetraInput.sTetraPad.GetVector() * 5;


        //ボタントリガー
        //if (buttonTrigger && usedButtonTrigger)
        //{
        //    buttonTrigger = false;
        //    usedButtonTrigger = false;
        //}
        //if (TetraInput.sTetraButton.GetTrigger())
        //{
        //    buttonTrigger = true;
        //}


        //if (TetraInput.sTetraButton.GetPress())
        //{
        //    buttonImage.texture = pressedbuttonTexture;
        //}
        //else
        //{
        //    buttonImage.texture = releasedbuttonTexture;
        //}

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (buttonTrigger)
        //{
        //    usedButtonTrigger = true;
        //    collision.attachedRigidbody?.AddForce((collision.transform.position - transform.position).normalized * impulseForce / Vector3.Distance(collision.transform.position, transform.position), ForceMode2D.Impulse);
        //}
    }
}
