using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTilemapPrefab
{
	Tilemap tilemap;
	public CustomTilemapPrefab(Tilemap tm)
	{
		tilemap = tm;
	}

	public void drawPrefab(Tilemap target, Vector3Int pos)
	{
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    
                    target.SetTile(new Vector3Int(x, y, 0) + pos, tile);
                }

            }
        }
    }
}
