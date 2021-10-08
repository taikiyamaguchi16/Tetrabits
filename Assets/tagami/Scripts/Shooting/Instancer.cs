using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instancer : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float instanceIntervalSeconds = 1.0f;
    float instanceTimer;
    [SerializeField] bool initTimerRandom;
    [SerializeField] int existLimit = 1;
    [SerializeField] List<GameObject> instancedObjects;

    private void Start()
    {
        instancedObjects = new List<GameObject>();

        if (initTimerRandom)
        {
            instanceTimer = Random.Range(0.0f, instanceIntervalSeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = instancedObjects.Count - 1; i >= 0; i--)
        {
            if (!instancedObjects[i])
            {
                instancedObjects.RemoveAt(i);
            }
        }

        if (instancedObjects.Count < existLimit)
        {
            instanceTimer += Time.deltaTime;
            if (instanceTimer >= instanceIntervalSeconds)
            {
                instanceTimer = 0.0f;
                var obj = Instantiate(prefab, transform.position, Quaternion.identity);
                instancedObjects.Add(obj);
            }
        }
    }
}
