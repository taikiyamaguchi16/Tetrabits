using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DummyStageMolder : MonoBehaviour
{
    //[SerializeField, Tooltip("敵のワープのつなぎ目")]
    //float enemyWarpPointZ = 50f;

    //public float EnemyWarpPointZ { get { return enemyWarpPointZ; } }

    public void DummyRoadMold(RaceStageMolder _raceStageMolder)
    {
        if (!Application.isPlaying)
        {

            // 旧ダミー消去
            GameObject[] children = new GameObject[gameObject.transform.childCount];
            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }

            for(int i = 0; i < children.Length; i++)
            {
                DestroyImmediate(children[i]);
            }

            // ステージ複製
            for (int i = 0; i < _raceStageMolder.GetLanes.Length; i++)
            {
                Instantiate(_raceStageMolder.GetLanes[i], this.gameObject.transform);
            }

            Instantiate(_raceStageMolder.GetOutfieldNearObj, this.gameObject.transform);
            Instantiate(_raceStageMolder.GetOutfieldBackObj, this.gameObject.transform);

            transform.localPosition = new Vector3(0, 0, _raceStageMolder.GetLaneLength);
        }
    }

}
