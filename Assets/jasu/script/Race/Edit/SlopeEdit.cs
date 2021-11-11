using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SlopeEdit : MonoBehaviour
{
    [SerializeField]
    GameObject road = null;

    [SerializeField]
    GameObject upSlope = null;

    [SerializeField]
    GameObject upSlopeCol = null;

    [SerializeField]
    GameObject upSlopeSprite = null;

    [SerializeField]
    GameObject downSlope = null;

    [SerializeField]
    GameObject downSlopeCol = null;

    [SerializeField]
    GameObject downSlopeSprite = null;

    [SerializeField]
    GameObject roadSprite = null;

    [SerializeField]
    SpriteRenderer roadSpriteRenderer;

    [Header("パラメータ調整")]

    [SerializeField, Tooltip("上の道の長さ")]
    float roadLength = 5f;

    [SerializeField, Tooltip("スロープの高さ")]
    float height = 5f;

    [SerializeField]
    float upSlopeAngle = 45f;

    [SerializeField]
    float downSlopeAngle = 45f;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            if (road != null &&
                 upSlope != null && upSlopeCol != null &&
                 downSlope != null && downSlopeCol != null)
            {
                // Roadの長さ、高さと位置をセット
                Vector3 scale = road.transform.localScale;
                scale.y = height;
                scale.z = roadLength;
                road.transform.localScale = scale;
                Vector3 pos = road.transform.localPosition;
                pos.y = height / 2;
                road.transform.localPosition = pos;

                // 坂の角度
                Vector3 angle = upSlope.transform.localRotation.eulerAngles;
                angle.x = -upSlopeAngle;
                upSlope.transform.localRotation = Quaternion.Euler(angle);

                angle = downSlope.transform.localRotation.eulerAngles;
                angle.x = downSlopeAngle;
                downSlope.transform.localRotation = Quaternion.Euler(angle);

                // 坂の長さ
                scale = upSlopeCol.transform.localScale;
                scale.z = height / Mathf.Sin(upSlopeAngle * Mathf.Deg2Rad);
                upSlopeCol.transform.localScale = scale;

                scale = downSlopeCol.transform.localScale;
                scale.z = height / Mathf.Sin(downSlopeAngle * Mathf.Deg2Rad);
                downSlopeCol.transform.localScale = scale;

                // 坂の位置
                pos = upSlope.transform.localPosition;
                pos.y = height / 2;
                pos.z = -((upSlopeCol.transform.localScale.z / 2) * Mathf.Cos(upSlopeAngle * Mathf.Deg2Rad) + roadLength / 2);
                upSlope.transform.localPosition = pos;

                pos = downSlope.transform.localPosition;
                pos.y = height / 2;
                pos.z = (downSlopeCol.transform.localScale.z / 2) * Mathf.Cos(downSlopeAngle * Mathf.Deg2Rad) + roadLength / 2;
                downSlope.transform.localPosition = pos;

                // スプライト対応
                if (upSlopeSprite != null &&
                    downSlopeSprite != null)
                {
                    // スケール
                    scale = upSlopeSprite.transform.localScale;
                    scale.x = Mathf.Cos(upSlopeAngle * Mathf.Deg2Rad) * upSlopeCol.transform.localScale.z;
                    scale.y = height;
                    upSlopeSprite.transform.localScale = scale;

                    scale = downSlopeSprite.transform.localScale;
                    scale.x = Mathf.Cos(downSlopeAngle * Mathf.Deg2Rad) * downSlopeCol.transform.localScale.z;
                    scale.y = height;
                    downSlopeSprite.transform.localScale = scale;

                    // 位置
                    pos = upSlopeSprite.transform.localPosition;
                    pos.y = height / 2;
                    pos.z = -((upSlopeCol.transform.localScale.z / 2) * Mathf.Cos(upSlopeAngle * Mathf.Deg2Rad) + roadLength / 2);
                    upSlopeSprite.transform.localPosition = pos;

                    pos = downSlopeSprite.transform.localPosition;
                    pos.y = height / 2;
                    pos.z = (downSlopeCol.transform.localScale.z / 2) * Mathf.Cos(downSlopeAngle * Mathf.Deg2Rad) + roadLength / 2;
                    downSlopeSprite.transform.localPosition = pos;
                }

                if (roadSprite != null &&
                    roadSpriteRenderer != null)
                {
                    pos = roadSprite.transform.localPosition;
                    pos.y = height;
                    roadSprite.transform.localPosition = pos;
                    roadSpriteRenderer.size = new Vector2(roadLength / 5, roadSpriteRenderer.size.y);
                }

            }
        }
    }
}
