﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSaver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    System.Diagnostics.Debug.WriteLine(tile.name);

                }

            }
        }
    }
}
