using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChainEnemyGenerator : MonoBehaviour
{
    [Header("Require Reference")]
    [SerializeField] ChainEnemyGenerator generator;

    [Header("Debug")]
    [SerializeField] bool hideRenderer = true;

    private void Awake()
    {
        if(!generator)
        {
            Debug.LogError("参照するジェネレーターを設定してください");
        }

        if(hideRenderer)
        {
            Renderer r;
            if(TryGetComponent(out r))
            {
                r.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        generator.Generate();
    }
}
