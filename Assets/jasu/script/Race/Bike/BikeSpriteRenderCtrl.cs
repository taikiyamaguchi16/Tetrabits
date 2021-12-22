using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSpriteRenderCtrl : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    //[SerializeField]
    //MoveBetweenLane moveBetweenLane = null;
    [SerializeField]
    RacerLaneShift racerLaneShift;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sortingOrder = 5 -(racerLaneShift.belongingLaneId * 10);
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = 5 - (racerLaneShift.belongingLaneId * 10);
    }
}
