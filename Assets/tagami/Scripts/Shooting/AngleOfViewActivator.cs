using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleOfViewActivator : MonoBehaviour
{
    [Header("Require Reference")]
    [SerializeField] List<Transform> objectsParents;


    [Header("Prefab Reference")]
    [SerializeField] Transform rightAreaTransform;

    private void Awake()
    {
        if (objectsParents.Count <= 0)
        {
            Debug.LogError("出現オブジェクトを設定してください");
        }

        foreach (var objectsParent in objectsParents)
        {
            for (int i = 0; i < objectsParent.childCount; i++)
            {
                objectsParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        foreach (var objectsParent in objectsParents)
        {
            for (int i = 0; i < objectsParent.childCount; i++)
            {
                if (!objectsParent.GetChild(i).gameObject.activeSelf && objectsParent.GetChild(i).position.x <= rightAreaTransform.position.x)
                {
                    objectsParent.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

}
