using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class StartLineSpriteMolder : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    RaceStageMolder raceStageMolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            if(raceStageMolder != null)
            {
                int laneNum = raceStageMolder.GetLanes.Length;

                Vector2 size = spriteRenderer.size;
                size.y = laneNum;
                spriteRenderer.size = size;

                Vector3 pos = transform.localPosition;
                pos.x = -(laneNum - 1)  * (raceStageMolder.GetRaceObjWidth / 2);
                transform.localPosition = pos; 
            }
        }
    }
}
