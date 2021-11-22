using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DummyStageMolder : MonoBehaviour
{
    [SerializeField]
    GameObject stageObj = null;

    private void Start()
    {
        if(Application.isPlaying && transform.GetChild(0) != null)
        {
            GameObject[] children = new GameObject[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < children.Length; i++)
            {
                Destroy(children[i]);
            }
            Instantiate(stageObj, this.gameObject.transform);
        }
    }

    public void DummyRoadMold()
    {
        // 旧ダミー消去
        GameObject[] children = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            children[i] = gameObject.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < children.Length; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(children[i]);
            }
            else
            {
                DestroyImmediate(children[i]);
            }
        }
        
        // ステージ複製
        Instantiate(stageObj, this.gameObject.transform);
        
        //for (int i = 0; i < _raceStageMolder.GetLanes.Length; i++)
        //{
        //    Instantiate(_raceStageMolder.GetLanes[i], this.gameObject.transform);
        //}
        
        //Instantiate(_raceStageMolder.GetOutfieldNearObj, this.gameObject.transform);
        //Instantiate(_raceStageMolder.GetOutfieldBackObj, this.gameObject.transform);
        
        //transform.localPosition = new Vector3(0, 0, _raceStageMolder.GetLaneLength);
    }

}
