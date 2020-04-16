using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLoader 
{
    public string assetName;
    TileProvider tp;
    public BoundsInt bounds;
    Tile[] tiles;
    Tile[] fgTiles;

    // Start is called before the first frame update
    public TilemapLoader(string name)
    {
        assetName = name;
        tp = GameObject.Find("TileProvider").GetComponent<TileProvider>();

        using (var sr = new StreamReader(assetName))
        {
            string s = sr.ReadLine();
            bounds = new BoundsInt(0, 0, 0, int.Parse(s.Split(' ')[0]), int.Parse(s.Split(' ')[1]), int.Parse(s.Split(' ')[2]));
            tiles = new Tile[bounds.size.x * bounds.size.y * bounds.size.z];
            fgTiles = new Tile[bounds.size.x * bounds.size.y * bounds.size.z];
            int c = 0;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    for (int k = 0; k < bounds.size.z; k++)
                    {
                        s = sr.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            var tile = tp.tiles[s];
                            if (k > 10) fgTiles[c] = tile;
                            else tiles[c] = tile;
                        }
                        c++;
                    }
                }
            }
        }      
    }

    public void paintConstruct(Tilemap map, Tilemap FGTilemap, Vector3Int pos)
    {
        BoundsInt b = bounds;
        b.position += pos;
        map.SetTilesBlock(b, tiles);
        b.position += new Vector3Int(0, 0, 100 * 100 - (b.y * 100) + (b.x * 10));
        FGTilemap.SetTilesBlock(b, fgTiles);
    }

}
