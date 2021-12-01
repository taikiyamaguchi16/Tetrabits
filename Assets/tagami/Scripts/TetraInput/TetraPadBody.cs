using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPadBody : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform spriteMaskTransform;
    [SerializeField] GameObject touchedPadEffectPrefab;
    

    public List<GameObject> onPadObjects { private set; get; }

    [HideInInspector] public bool creatableEffect;

    private void Awake()
    {
        onPadObjects = new List<GameObject>();
    }

    private void Update()
    {
        //存在しないオブジェクトは除去
        onPadObjects.RemoveAll(s => !s);
    }

    private void FixedUpdate()
    {
        //物理処理前にクリアしておく
        onPadObjects.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryToAddOnPadObject(collision.gameObject);

        //エフェクト生成
        if (creatableEffect)
        {
            var effectObj = Instantiate(touchedPadEffectPrefab);
            effectObj.transform.position = new Vector3(collision.transform.position.x, spriteMaskTransform.position.y, collision.transform.position.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        TryToAddOnPadObject(collision.gameObject);
    }

    private bool TryToAddOnPadObject(GameObject _go)
    {
        foreach (var obj in onPadObjects)
        {
            if (obj == _go)
                return false;
        }

        onPadObjects.Add(_go);
        return true;
    }

    private void OnCollisionExit(Collision collision)
    {
        onPadObjects.Remove(collision.gameObject);
    }
}
