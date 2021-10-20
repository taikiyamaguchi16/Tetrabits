using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GimmickController : MonoBehaviour
{
    Tilemap tilemap;
    TilemapCollider2D tilemapCollider;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = this.gameObject.GetComponent<Tilemap>();
        tilemapCollider = this.gameObject.GetComponent<TilemapCollider2D>();
        tilemap.color = new Color(1, 1, 1, 0.1f);
        tilemapCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            tilemap.color = new Color(1, 1, 1, 1);
            tilemapCollider.enabled = true;
        }
        else if (!TetraInput.sTetraLever.GetPoweredOn())
        {
            tilemap.color = new Color(1, 1, 1, 0.1f);
            tilemapCollider.enabled = false;
        }
    }
}