using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoomLoader
{
    public GameObject prefab;
    public float rerollChance;
    public string name;
    TileProvider tp;
    public BoundsInt bounds;
    Tile[] tiles;
    Tile[] fgTiles;
    Tile[] fullFgTiles;

    Vector2Int origin;

    List<DungeonDoorMarker> doors;

    public List<DungeonDoorMarker> getDoors() => doors.Select(t => new DungeonDoorMarker() { corridorChance = 0.7f, intoDir = t.intoDir, open = true, pos = t.pos }).ToList();


    public DungeonRoomLoader(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;

        rerollChance = prefab.GetComponent<DungeonRoom>().rerollChance;

        origin = Vector2Int.FloorToInt(prefab.transform.position);

        tp = GameObject.Find("TileProvider").GetComponent<TileProvider>();

        doors = this.prefab.GetComponentsInChildren<DungeonDoorMarker>().ToList();
        doors.ForEach(t => t.pos = Vector2Int.FloorToInt(new Vector2(t.transform.localPosition.x, t.transform.localPosition.y)));

        using (var sr = new StreamReader(name))
        {
            string s;
            var bottomLeft = getv2(sr.ReadLine(), 0);
            var topRight = getv2(sr.ReadLine(), 16);
            bounds = new BoundsInt(bottomLeft, topRight - bottomLeft);
            tiles = new Tile[bounds.size.x * bounds.size.y * 16];
            fgTiles = new Tile[bounds.size.x * bounds.size.y * 16];
            fullFgTiles = new Tile[bounds.size.x * bounds.size.y * 16];
            int c = 0;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        s = sr.ReadLine();

                        c = k + i * 16 + j * bounds.size.x * 16;

                        if (!string.IsNullOrEmpty(s))
                        {
                            tiles[c] = tp.tiles[s];

                        }
                        s = sr.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            fgTiles[c] = tp.tiles[s];

                        }
                        s = sr.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            fullFgTiles[c] = tp.tiles[s];

                        }
                        
                    }
                }
            }
        }
    }

    public Vector2Int getGlobalPosByDoor(DungeonDoorMarker myDoor, Vector2Int worldDoor)
    {
        var door = doors.FirstOrDefault(t => t.pos == myDoor.pos);
        if (door == null) return Vector2Int.zero;
        else return worldDoor - door.pos;
    }

    public Vector2Int getTranslation(Vector2Int worldPos)
    {
        return worldPos - origin;
    }

    static Vector3Int getv2(string s, int z)
    {
        s = s.Replace("(", "").Replace(")", "").Replace(", "," ").Replace(".0","");
        return new Vector3Int(int.Parse(s.Split(' ')[0]), int.Parse(s.Split(' ')[1]), z);
    }

    public BoundsInt getWorldBounds(Vector2Int pos)
    {
        BoundsInt b = bounds;
        Vector2Int translation = getTranslation(new Vector2Int(pos.x, pos.y));
        b.position += new Vector3Int(translation.x, translation.y, 0);
        return b;
    }

    public void paintConstruct(Tilemap map, Tilemap FGTilemap, Tilemap FullFGTilemap, Vector3Int pos)
    {
        BoundsInt b = bounds;
        Vector2Int translation = getTranslation(new Vector2Int(pos.x, pos.y));
        b.position += new Vector3Int(translation.x, translation.y, 0);
        map.SetTilesBlock(b, tiles);
        FGTilemap.SetTilesBlock(b, fgTiles);
        FullFGTilemap.SetTilesBlock(b, fullFgTiles);
    }
}
