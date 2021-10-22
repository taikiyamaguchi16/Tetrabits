using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GimmickController : MonoBehaviour
{
    Tilemap tilemap;
    TilemapCollider2D tilemapCollider;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = this.gameObject.GetComponent<Tilemap>();
        tilemapCollider = this.gameObject.GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leverState!= TetraInput.sTetraLever.GetPoweredOn())
        {
            SwitchGimmick();
        }
        leverState = TetraInput.sTetraLever.GetPoweredOn();
    }

    void SwitchGimmick()
    {
        if(tilemapCollider.enabled)
        {
            tilemap.color = new Color(1, 1, 1, 0.1f);
            tilemapCollider.enabled = false;
        }
        else
        {
            tilemap.color = new Color(1, 1, 1, 1);
            tilemapCollider.enabled = true;
        }
    }
}