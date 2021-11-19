using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting
{
    public class TransformSynchronizer : MonoBehaviour
    {
        public GameObject targetObject;

        [SerializeField] bool synchroPosition = true;
        [SerializeField] bool synchroRotation = true;
        [SerializeField] bool synchroLocalScale = false;

        // Update is called once per frame
        void LateUpdate()
        {
            if (targetObject)
            {
                if (synchroPosition)
                {
                    transform.position = targetObject.transform.position;
                }
                if (synchroRotation)
                {
                    transform.rotation = targetObject.transform.rotation;
                }
                if(synchroLocalScale)
                {
                    transform.localScale = targetObject.transform.localScale;
                }
            }
        }
    }
}
