using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorTimeElapsed : MonoBehaviour
{
    [System.Serializable]
    class GenerateStatus
    {
        public GameObject generatePrefab;
        public float generateSeconds;
        public bool generated;
    }

    [SerializeField] List<GenerateStatus> generateStatusList;

    float generateTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        generateTimer += Time.deltaTime;

        foreach (var status in generateStatusList)
        {
            if (!status.generated && generateTimer >= status.generateSeconds)
            {
                //ジェネレーターがある場所に生成
                Instantiate(status.generatePrefab, transform.position, Quaternion.identity, transform);

                status.generated = true;
            }
        }
    }
}
