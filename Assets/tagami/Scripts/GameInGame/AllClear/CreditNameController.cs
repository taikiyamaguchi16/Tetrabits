using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CreditNameController : MonoBehaviour
{
    [Header("Bit")]
    [SerializeField] GameObject creditNameBitPrefab;

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

        var myShadow = GetComponent<Shadow>();

        //一文字ずつ作成
        float rectLocalScalePerByte = dividedText.fontSize * 0.55f;
        Vector3 instanceLocalPosition = new Vector3(-(GetComponent<RectTransform>().sizeDelta.x / 2) + (dividedText.fontSize / 2), 0, 0);
        foreach (var str in dividedText.text)
        {
            var obj = Instantiate(creditNameBitPrefab, transform);

            //Name
            obj.name = str.ToString();

            //Transform
            var rectTrans = obj.GetComponent<RectTransform>();
            rectTrans.localPosition = instanceLocalPosition;
            rectTrans.localScale = Vector3.one;
            var strByte = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(str.ToString());
            rectTrans.sizeDelta = new Vector2(rectLocalScalePerByte * strByte, rectLocalScalePerByte * 2);

            //Text
            var text = obj.GetComponent<Text>();
            text.text = str.ToString();
            text.font = dividedText.font;
            text.fontSize = dividedText.fontSize;
            text.color = dividedText.color;

            //Collider
            //var boxCollider2D = obj.AddComponent<BoxCollider2D>();
            //boxCollider2D.size = rectTrans.sizeDelta;
            var circleCollider2D = obj.GetComponent<CircleCollider2D>();
            circleCollider2D.radius = rectTrans.sizeDelta.x / 2;
            //var sphereCollider = obj.GetComponent<SphereCollider>();
            //sphereCollider.radius = rectTrans.sizeDelta.x / 2;

            //Shadow
            if (myShadow)
            {
                var shadow = obj.AddComponent<Shadow>();
                shadow.effectColor = myShadow.effectColor;
                shadow.effectDistance = myShadow.effectDistance;
            }

            //次への準備
            instanceLocalPosition.x += rectTrans.sizeDelta.x;
        }//分割

        dividedText.enabled = false;

    }
}
