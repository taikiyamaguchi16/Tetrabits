using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderOnRoadCtrl : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] spriteRenderers;

    [SerializeField]
    LaneInfo laneInfo = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();


        if (laneInfo != null ||
            (laneInfo = GetComponentInParent<LaneInfo>()) != null)
        {
            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.sortingOrder += -(laneInfo.laneId * 10);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
