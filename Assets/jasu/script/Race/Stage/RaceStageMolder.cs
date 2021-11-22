using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RaceStageMolder : MonoBehaviour
{
    [SerializeField]
    GameObject[] lanes;

    public GameObject[] GetLanes { get { return lanes; } }

    [SerializeField]
    RaceObjInfo outfieldNear = null;

    public GameObject GetOutfieldNearObj { get { return outfieldNear.gameObject; } }

    [SerializeField]
    RaceObjInfo outfieldBack = null;

    public GameObject GetOutfieldBackObj { get { return outfieldBack.gameObject; } }

    [SerializeField]
    float raceObjWidth = 5f;

    public float GetRaceObjWidth { get { return raceObjWidth; } }

    [SerializeField]
    DummyStageMolder dummyRoadMolder = null;

    public DummyStageMolder GetDummyRoadMolder { get { return dummyRoadMolder; } }

    [Header("設定項目")]

    [SerializeField]
    float laneWidthMultiply = 1f;

    public float GetLaneWidthMultiply { get { return laneWidthMultiply; } }

    [SerializeField, Tooltip("コースの長さ")]
    float laneLength = 800f;

    public float GetLaneLength { get { return laneLength; } }

    [SerializeField]
    int outfieldNearWidthNum = 4;

    public float GetOutfieldNearWidthNum { get { return outfieldNearWidthNum; } }

    [SerializeField]
    int outfieldBackWidthNum = 8;

    public float GetOutfieldBackWidthNum { get { return outfieldBackWidthNum; } }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < lanes.Length; i++)
            {
                lanes[i].GetComponent<LaneInfo>().laneId = i;
            }
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (lanes.Length > 0)
            {
                for (int i = 0; i < lanes.Length; i++)
                {
                    lanes[i].transform.localPosition = new Vector3(-raceObjWidth * laneWidthMultiply * i, -raceObjWidth / 2, 0);
                    RaceObjInfo roadInfo = lanes[i].GetComponent<RaceObjInfo>();

                    // コライダー調整
                    Vector3 scale = roadInfo.colliderObj.transform.localScale;
                    scale.x = raceObjWidth * laneWidthMultiply;
                    scale.z = laneLength;
                    roadInfo.colliderObj.transform.localScale = scale;

                    Vector3 pos = roadInfo.colliderObj.transform.localPosition;
                    pos.z = laneLength / 2;
                    roadInfo.colliderObj.transform.localPosition = pos;

                    // スプライト調整
                    scale = roadInfo.spriteRenderer.transform.localScale;
                    scale.x = raceObjWidth * laneWidthMultiply;
                    scale.y = raceObjWidth * laneWidthMultiply;
                    roadInfo.spriteRenderer.transform.localScale = scale;

                    roadInfo.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), 1);

                    pos = roadInfo.spriteRenderer.transform.localPosition;
                    pos.z = laneLength / 2;
                    roadInfo.spriteRenderer.transform.localPosition = pos;
                }
            }

            // 外野手前
            if (outfieldNear != null)
            {
                outfieldNear.transform.localPosition = new Vector3(raceObjWidth * laneWidthMultiply / 2, -raceObjWidth / 2, 0);

                // コライダー
                Vector3 scale = outfieldNear.colliderObj.transform.localScale;
                scale.x = raceObjWidth * laneWidthMultiply * outfieldNearWidthNum;
                scale.z = laneLength;
                outfieldNear.colliderObj.transform.localScale = scale;

                Vector3 pos = outfieldNear.colliderObj.transform.localPosition;
                pos.x = scale.x / 2;
                pos.z = laneLength / 2;
                outfieldNear.colliderObj.transform.localPosition = pos;

                // スプライト
                pos = outfieldNear.spriteRenderer.transform.localPosition;
                pos.x = scale.x / 2;
                pos.z = laneLength / 2;
                outfieldNear.spriteRenderer.transform.localPosition = pos;

                scale = outfieldNear.spriteRenderer.transform.localScale;
                scale.x = raceObjWidth * laneWidthMultiply;
                scale.y = raceObjWidth * laneWidthMultiply;
                outfieldNear.spriteRenderer.transform.localScale = scale;

                outfieldNear.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), outfieldNearWidthNum);
            }

            // 外野奥側
            if (outfieldBack != null)
            {
                outfieldBack.transform.localPosition = new Vector3(-(raceObjWidth * laneWidthMultiply * lanes.Length) + raceObjWidth * laneWidthMultiply / 2, -raceObjWidth / 2, 0);

                // コライダー
                Vector3 scale = outfieldBack.colliderObj.transform.localScale;
                scale.x = raceObjWidth * laneWidthMultiply * outfieldBackWidthNum;
                scale.z = laneLength;
                outfieldBack.colliderObj.transform.localScale = scale;

                Vector3 pos = outfieldBack.colliderObj.transform.localPosition;
                pos.x = -scale.x / 2;
                pos.z = laneLength / 2;
                outfieldBack.colliderObj.transform.localPosition = pos;

                // スプライト
                pos = outfieldBack.spriteRenderer.transform.localPosition;
                pos.x = -scale.x / 2;
                pos.z = laneLength / 2;
                outfieldBack.spriteRenderer.transform.localPosition = pos;

                scale = outfieldBack.spriteRenderer.transform.localScale;
                scale.x = raceObjWidth * laneWidthMultiply;
                scale.y = raceObjWidth * laneWidthMultiply;
                outfieldBack.spriteRenderer.transform.localScale = scale;

                outfieldBack.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), outfieldBackWidthNum);
            }

            // 周回用のダミーの道を生成
            if (dummyRoadMolder != null)
            {
                dummyRoadMolder.transform.localPosition = new Vector3(0, 0, laneLength);
                dummyRoadMolder.DummyRoadMold();
            }
        }
    }

    //private void OnEnable()
    //{
    //    if (!Application.isPlaying)
    //    {
    //        if (lanes.Length > 0)
    //        {
    //            for (int i = 0; i < lanes.Length; i++)
    //            {
    //                lanes[i].transform.localPosition = new Vector3(-raceObjWidth * laneWidthMultiply * i, 0, 0);
    //                RaceObjInfo roadInfo = lanes[i].GetComponent<RaceObjInfo>();

    //                // コライダー調整
    //                Vector3 scale = roadInfo.colliderObj.transform.localScale;
    //                scale.x = raceObjWidth * laneWidthMultiply;
    //                scale.z = laneLength;
    //                roadInfo.colliderObj.transform.localScale = scale;

    //                Vector3 pos = roadInfo.colliderObj.transform.localPosition;
    //                pos.z = laneLength / 2;
    //                roadInfo.colliderObj.transform.localPosition = pos;

    //                // スプライト調整
    //                scale = roadInfo.spriteRenderer.transform.localScale;
    //                scale.x = raceObjWidth * laneWidthMultiply;
    //                scale.y = raceObjWidth * laneWidthMultiply;
    //                roadInfo.spriteRenderer.transform.localScale = scale;

    //                roadInfo.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), 1);

    //                pos = roadInfo.spriteRenderer.transform.localPosition;
    //                pos.z = laneLength / 2;
    //                roadInfo.spriteRenderer.transform.localPosition = pos;
    //            }
    //        }

    //        // 外野手前
    //        if (outfieldNear != null)
    //        {
    //            outfieldNear.transform.localPosition = new Vector3(raceObjWidth * laneWidthMultiply / 2, 0, 0);

    //            // コライダー
    //            Vector3 scale = outfieldNear.colliderObj.transform.localScale;
    //            scale.x = raceObjWidth * laneWidthMultiply * outfieldNearWidthNum;
    //            scale.z = laneLength;
    //            outfieldNear.colliderObj.transform.localScale = scale;

    //            Vector3 pos = outfieldNear.colliderObj.transform.localPosition;
    //            pos.x = scale.x / 2;
    //            pos.z = laneLength / 2;
    //            outfieldNear.colliderObj.transform.localPosition = pos;

    //            // スプライト
    //            pos = outfieldNear.spriteRenderer.transform.localPosition;
    //            pos.x = scale.x / 2;
    //            pos.z = laneLength / 2;
    //            outfieldNear.spriteRenderer.transform.localPosition = pos;

    //            scale = outfieldNear.spriteRenderer.transform.localScale;
    //            scale.x = raceObjWidth * laneWidthMultiply;
    //            scale.y = raceObjWidth * laneWidthMultiply;
    //            outfieldNear.spriteRenderer.transform.localScale = scale;

    //            outfieldNear.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), outfieldNearWidthNum);
    //        }

    //        // 外野奥側
    //        if (outfieldBack != null)
    //        {
    //            outfieldBack.transform.localPosition = new Vector3(-(raceObjWidth * laneWidthMultiply * lanes.Length) + raceObjWidth * laneWidthMultiply / 2, 0, 0);

    //            // コライダー
    //            Vector3 scale = outfieldBack.colliderObj.transform.localScale;
    //            scale.x = raceObjWidth * laneWidthMultiply * outfieldBackWidthNum;
    //            scale.z = laneLength;
    //            outfieldBack.colliderObj.transform.localScale = scale;

    //            Vector3 pos = outfieldBack.colliderObj.transform.localPosition;
    //            pos.x = -scale.x / 2;
    //            pos.z = laneLength / 2;
    //            outfieldBack.colliderObj.transform.localPosition = pos;

    //            // スプライト
    //            pos = outfieldBack.spriteRenderer.transform.localPosition;
    //            pos.x = -scale.x / 2;
    //            pos.z = laneLength / 2;
    //            outfieldBack.spriteRenderer.transform.localPosition = pos;

    //            scale = outfieldBack.spriteRenderer.transform.localScale;
    //            scale.x = raceObjWidth * laneWidthMultiply;
    //            scale.y = raceObjWidth * laneWidthMultiply;
    //            outfieldBack.spriteRenderer.transform.localScale = scale;

    //            outfieldBack.spriteRenderer.size = new Vector2(laneLength / (raceObjWidth * laneWidthMultiply), outfieldBackWidthNum);
    //        }

    //        // 周回用のダミーの道を生成
    //        if (dummyRoadMolder != null)
    //        {
    //            dummyRoadMolder.DummyRoadMold(this);
    //        }
    //    }
    //}
}
