using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class LerpTester : MonoBehaviour
    {
        [SerializeField] Vector3 startLocalPosition;
        [SerializeField] Vector3 endLocalPosition;
        [SerializeField] float dt;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, dt);
            transform.localPosition = Lerp(startLocalPosition, endLocalPosition, dt);

        }

        Vector3 Lerp(Vector3 _start,Vector3 _end,float _dt)
        {
            Vector3 ret = new Vector3();
            ret.x = _start.x + _dt * (_end.x - _start.x);
            ret.y = _start.y + _dt * (_end.y - _start.y);
            ret.z = _start.z + _dt * (_end.z - _start.z);
            return ret;
        }
    }
}
