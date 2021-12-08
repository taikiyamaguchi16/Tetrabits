using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPadBody : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] Transform spriteMaskTransform;
    [SerializeField] GameObject touchedPadEffectPrefab;
    
    //使ってもらう
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
        if (TryToAddOnPadObject(collision.gameObject))
        {
            //エフェクト生成
            if (creatableEffect)
            {
                var effectObj = Instantiate(touchedPadEffectPrefab);
                effectObj.transform.position = new Vector3(collision.transform.position.x, spriteMaskTransform.position.y, collision.transform.position.z);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        TryToAddOnPadObject(collision.gameObject);
    }

    private bool TryToAddOnPadObject(GameObject _go)
    {
        //同じオブジェクトははじく
        foreach (var obj in onPadObjects)
        {
            if (obj == _go)
                return false;
        }
        //タグがPlayerでないならはじく
        if(!_go.CompareTag("Player"))
        {
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
