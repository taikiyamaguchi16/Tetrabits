using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectCpt : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameObjectList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyObject()
    {
        for(int i= 0; i < gameObjectList.Count; i++)
        {
            Destroy(gameObjectList[i]);
        }
        Destroy(this);
    }
}
