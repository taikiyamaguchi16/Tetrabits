using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleOfViewActivator : MonoBehaviour
{
    [Header("Require Reference")]
    [SerializeField] Transform objectsParent;


    [Header("Prefab Reference")]
    [SerializeField] Transform rightAreaTransform;

    private void Start()
    {
        for (int i = 0; i < objectsParent.childCount; i++)
        {
            objectsParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update()
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
