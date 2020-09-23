using Assets.Scenes.Wayfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StaticDungeon : MonoBehaviour, IMapEnabled
{
    protected MapModule map;
    protected Tilemap tilemap;
    protected Tilemap FGTilemap;
    protected Tilemap FullFGTilemap;
    protected TileProvider tp;
    public int w, h, floorIreggularityNum;
    public Tile defaultFloor;
    public Tile defaultVoid;

    List<TilemapLoader> floors = new List<TilemapLoader>();

    public virtual int getH()
    {
        return w;
    }

    public virtual int getW()
    {
        return h;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        preSetup();
        postSetup();
    }

    protected virtual void preSetup()
    {
        map = GetComponent<MapModule>();
        tp = GameObject.Find("TileProvider").GetComponent<TileProvider>();
        tilemap = gameObject.transform.Find("Tilemap").GetComponent<Tilemap>();
        FGTilemap = gameObject.transform.Find("Foreground").GetComponent<Tilemap>();
        FullFGTilemap = gameObject.transform.Find("FullFG").GetComponent<Tilemap>();
    }

    protected virtual void postSetup()
    {
        BoundsInt bounds = tilemap.cellBounds;
        var tiles = tilemap.GetTilesBlock(bounds);
        int c = 0;
        for (int i = 0; i < bounds.size.y; i++)
        {
            for (int j = 0; j < bounds.size.x; j++)
            {
                for (int k = 0; k < bounds.size.z; k++)
                {
                    if (tiles[c] != null && tp.tiles[tiles[c].name].colliderType != Tile.ColliderType.None)
                    {
                        map.map[bounds.x + j, bounds.y + i] = true;
                    }
                    c++;
                }
            }
        }
        bounds = FGTilemap.cellBounds;
        tiles = FGTilemap.GetTilesBlock(bounds);
        c = 0;
        for (int i = 0; i < bounds.size.y; i++)
        {
            for (int j = 0; j < bounds.size.x; j++)
            {
                for (int k = 0; k < bounds.size.z; k++)
                {
                    if (tiles[c] != null && tp.tiles[tiles[c].name].colliderType != Tile.ColliderType.None)
                    {
                        map.map[bounds.x + j, bounds.y + i] = true;
                    }
                    c++;
                }
            }
        }
        loadConstructs();
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), defaultFloor);
            }
        }
        for (int i = 0; i < floorIreggularityNum; i++)
        {
            floors[Random.Range(0, floors.Count)].paintConstruct(tilemap, tilemap, (new Vector3Int(Random.Range(0, w), Random.Range(0, h), 1)));
        }
    }

    void loadConstructs()
    {
        foreach (var item in Directory.GetFiles("Assets\\TileConstructs", "*.tile.construct"))
        {
            var key = item.Split('\\').Last().Split('.')[0];
            if (key.StartsWith("DungeonFloor")) floors.Add(new TilemapLoader(item));
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
