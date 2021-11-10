using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SlopeEdit : MonoBehaviour
{
    [SerializeField]
    GameObject road = null;

    [SerializeField]
    GameObject upSlope = null;

    [SerializeField]
    GameObject upSlopeCol = null;

    [SerializeField]
    GameObject downSlope = null;

    [SerializeField]
    GameObject downSlopeCol = null;

    [Header("パラメータ調整")]

    [SerializeField, Tooltip("上の道の長さ")]
    float roadLength = 5f;

    [SerializeField, Tooltip("スロープの高さ")]
    float height = 5f;

    [SerializeField]
    float slopeAngle = 45f;

    private void OnEnable()
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
            angle.x = -slopeAngle;
            upSlope.transform.localRotation = Quaternion.Euler(angle);

            angle = downSlope.transform.localRotation.eulerAngles;
            angle.x = slopeAngle;
            downSlope.transform.localRotation = Quaternion.Euler(angle);
            
            // 坂の長さ
            scale = upSlopeCol.transform.localScale;
            scale.z = height / Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
            upSlopeCol.transform.localScale = scale;
            downSlopeCol.transform.localScale = scale;

            // 坂の位置
            pos = upSlope.transform.localPosition;
            pos.y = height / 2;
            pos.z = -((upSlopeCol.transform.localScale.z / 2) * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) + roadLength / 2);
            upSlope.transform.localPosition = pos;

            pos = downSlope.transform.localPosition;
            pos.y = height / 2;
            pos.z = (downSlopeCol.transform.localScale.z / 2) * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) + roadLength / 2;
            downSlope.transform.localPosition = pos;
        }
    }
}
