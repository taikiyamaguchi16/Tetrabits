using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CreditNameController : MonoBehaviour
{
    Text dividedText;
    List<Rigidbody2D> createdCharRigidbodys = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        dividedText = GetComponent<Text>();
        if (dividedText.text == null || dividedText.text.Length <= 0)
        {
            Debug.LogError("文字列を設定してください");
            return;
        }

        //一文字ずつ作成
        var rectLocalScale = new Vector2(dividedText.fontSize * 1.1f, dividedText.fontSize * 1.1f);
        Vector3 instanceLocalPosition = new Vector3(-rectLocalScale.x * dividedText.text.Length / 2, 0, 0);
        foreach (var str in dividedText.text)
        {
            var obj = new GameObject(str.ToString());

            //Transform
            var rectTrans = obj.AddComponent<RectTransform>();
            rectTrans.parent = this.transform;
            rectTrans.localPosition = instanceLocalPosition;
            rectTrans.localScale = Vector3.one;
            rectTrans.sizeDelta = rectLocalScale;

            //Text
            var text = obj.AddComponent<Text>();
            text.text = str.ToString();
            text.font = dividedText.font;
            text.fontSize = dividedText.fontSize;
            text.color = dividedText.color;

            //Collider
            //var boxCollider2D = obj.AddComponent<BoxCollider2D>();
            //boxCollider2D.size = rectTrans.sizeDelta;
            var circleCollider2D = obj.AddComponent<CircleCollider2D>();
            circleCollider2D.radius = rectTrans.sizeDelta.x / 2;

            //Rigidbody
            var rb = obj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;

            //登録
            createdCharRigidbodys.Add(rb);

            //次への準備
            instanceLocalPosition.x += rectLocalScale.x;
        }//分割

        dividedText.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        //横移動
        transform.position += -Vector3.right * Time.deltaTime;

        if (TetraInput.sTetraButton.GetTrigger())
        {
            foreach (var rb in createdCharRigidbodys)
            {
                rb.AddForce(rb.transform.localPosition * 5.0f);
            }
        }
    }
}
