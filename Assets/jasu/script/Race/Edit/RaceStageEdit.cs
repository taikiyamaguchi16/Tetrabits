using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RaceStageEdit : MonoBehaviour
{
    [SerializeField]
    GameObject[] lanes;

    [SerializeField]
    RaceObjInfo outfieldNear = null;

    [SerializeField]
    RaceObjInfo outfieldBack = null;

    float width = 5f;
    

    [Header("パラメータ調整")]

    [SerializeField]
    float laneWidthMultiply = 1f;

    [SerializeField]
    float laneLength = 800f;

    [SerializeField]
    int outfieldNearWidthNum = 4;

    [SerializeField]
    int outfieldBackWidthNum = 8;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            if (lanes.Length > 0)
            {
                for (int i = 0; i < lanes.Length; i++)
                {
                    lanes[i].transform.localPosition = new Vector3(-width * laneWidthMultiply * i, 0, 0);
                    RaceObjInfo roadInfo = lanes[i].GetComponent<RaceObjInfo>();

                    // コライダー調整
                    Vector3 scale = roadInfo.colliderObj.transform.localScale;
                    scale.x = width * laneWidthMultiply;
                    scale.z = laneLength;
                    roadInfo.colliderObj.transform.localScale = scale;

                    Vector3 pos = roadInfo.colliderObj.transform.localPosition;
                    pos.z = laneLength / 2;
                    roadInfo.colliderObj.transform.localPosition = pos;

                    // スプライト調整
                    scale = roadInfo.spriteRenderer.transform.localScale;
                    scale.x = width * laneWidthMultiply;
                    scale.y = width * laneWidthMultiply;
                    roadInfo.spriteRenderer.transform.localScale = scale;

                    roadInfo.spriteRenderer.size = new Vector2(laneLength / (width * laneWidthMultiply), 1);

                    pos = roadInfo.spriteRenderer.transform.localPosition;
                    pos.z = laneLength / 2;
                    roadInfo.spriteRenderer.transform.localPosition = pos;
                }
            }

            if (outfieldNear != null)
            {
                outfieldNear.transform.localPosition = new Vector3(width * laneWidthMultiply / 2, 0, 0);

                // コライダー
                Vector3 scale = outfieldNear.colliderObj.transform.localScale;
                scale.x = width * laneWidthMultiply * outfieldNearWidthNum;
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
                scale.x = width * laneWidthMultiply;
                scale.y = width * laneWidthMultiply;
                outfieldNear.spriteRenderer.transform.localScale = scale;

                outfieldNear.spriteRenderer.size = new Vector2(laneLength / (width * laneWidthMultiply), outfieldNearWidthNum);
            }

            if (outfieldBack != null)
            {
                outfieldBack.transform.localPosition = new Vector3(-(width * laneWidthMultiply * lanes.Length) + width * laneWidthMultiply / 2, 0, 0);

                // コライダー
                Vector3 scale = outfieldBack.colliderObj.transform.localScale;
                scale.x = width * laneWidthMultiply * outfieldBackWidthNum;
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
                scale.x = width * laneWidthMultiply;
                scale.y = width * laneWidthMultiply;
                outfieldBack.spriteRenderer.transform.localScale = scale;

                outfieldBack.spriteRenderer.size = new Vector2(laneLength / (width * laneWidthMultiply), outfieldBackWidthNum);
            }
        }
    }
}
