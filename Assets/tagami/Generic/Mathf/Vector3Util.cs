using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic.Utility
{
    public static class Vector3Util
    {
        public static Vector3 LerpWaypoints(List<Transform> _waypoints, float _single)
        {
            Vector3 pos;
            Quaternion qt;
            Vector3 scale;
            MathfUtil.LerpWaypoints(_waypoints, _single, out pos, out qt, out scale);
            return pos;
        }
    }

}