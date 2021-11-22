using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting
{
    public class AngleOfViewActivator : MonoBehaviour
    {
        [Header("Require Reference")]
        [SerializeField] List<Transform> objectsParents;

        [Header("Prefab Reference")]
        [SerializeField] Transform rightAreaTransform;
        [SerializeField] Transform leftAreaTransform;

        bool initializedUpdate;

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
                    if (!objectsParent.GetChild(i).gameObject.CompareTag("NotAffectedActivator"))
                    {
                        objectsParent.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }

        private void Update()
        {
            foreach (var objectsParent in objectsParents)
            {
                for (int i = 0; i < objectsParent.childCount; i++)
                {
                    var child = objectsParent.GetChild(i).gameObject;

                    //こいつは無視する
                    if (child.CompareTag("NotAffectedActivator"))
                    {
                        continue;
                    }

                       
                    if (!child.activeSelf && child.transform.position.x <= rightAreaTransform.position.x)
                    {
                        //一回目は削除することで途中からデバッグできる
                        if (!initializedUpdate)
                        {
                            Destroy(child);                           
                        }
                        else
                        {
                            //有効化
                            child.SetActive(true);
                            //インターフェースを呼ぶ
                            IActivate activate;
                            if (objectsParent.GetChild(i).gameObject.TryGetComponent(out activate))
                            {
                                activate.OnActivated();
                            }
                        }
                    }
                    if (child.transform.position.x <= leftAreaTransform.position.x)
                    {
                        Destroy(child);
                    }

                }
            }

            if(!initializedUpdate)
            {
                initializedUpdate = true;
            }
        }//update
    }

}//namespace
