using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTrackerUI : MonoBehaviour
{
    [SerializeField]
    Image myBody;

    [SerializeField]
    Transform target;
    
    RacerInfo targetRacerInfo;

    [SerializeField]
    Transform playerTrans;
    
    RacerInfo playerRacerInfo;

    Camera mainCamera;

    Rect rect = new Rect(0, 0, 1, 1);

    Rect canvasRect;
    
    [SerializeField]
    float showDistance = 100f;

    void Start()
    {
        mainCamera = Camera.main;
        targetRacerInfo = target.GetComponent<RacerInfo>();
        playerRacerInfo = playerTrans.GetComponent<RacerInfo>();

        // UIがはみ出ないようにする
        canvasRect = ((RectTransform)myBody.canvas.transform).rect;
        canvasRect.Set(
            canvasRect.x + myBody.rectTransform.rect.width * 0.5f,
            canvasRect.y + myBody.rectTransform.rect.height * 0.5f,
            canvasRect.width - myBody.rectTransform.rect.width,
            canvasRect.height - myBody.rectTransform.rect.height
        );
    }

    void LateUpdate()
    {
        float distance = Mathf.Abs(playerTrans.position.z - target.position.z);

        var viewport = mainCamera.WorldToViewportPoint(target.position);
        if (rect.Contains(viewport) || distance > showDistance)
        {
            myBody.enabled = false;
        }
        else
        {
            myBody.enabled = true;

            // 画面内で対象を追跡
            if(targetRacerInfo.ranking < playerRacerInfo.ranking)
            {
                viewport.x = Mathf.Clamp01(100);
                myBody.rectTransform.localEulerAngles = Vector3.zero;
            }
            else
            {
                viewport.x = Mathf.Clamp01(-100);
                myBody.rectTransform.localEulerAngles = new Vector3(0, 180, 0);
            }
            
            viewport.y = Mathf.Clamp01(viewport.y);
            
            myBody.rectTransform.anchoredPosition = Rect.NormalizedToPoint(canvasRect, viewport);
        }
    }
}
