using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonCreator : MonoBehaviour
{
    public Tile tile;
    public Tile wall;
    public Tile floor;
    Tilemap tilemap;
    bool[,] map = new bool[1000, 1000];

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
       // tilemap.size = new Vector3Int(10000, 10000, 1);
        int fails = 0;

      
        while (fails < 100)
        {
            Room r = new Room();
            Vector3Int pos = new Vector3Int(Random.Range(0, 1000 - r.width), Random.Range(0, 1000 - r.height), 0);
            if (!checkRoom(r, pos)) fails++;
            else
            {
                var mask = r.getShape();
                for (int i = 0; i < mask.GetLength(0); i++)
                {
                    for (int j = 0; j < mask.GetLength(1); j++)
                    {
                        if (mask[i, j])
                        {
                            map[i + pos.x, j + pos.y] = true;
                        }
                    }
                }
                r.drawRoom(tilemap, wall, floor, pos);
            }
        }
    }

    bool checkRoom(Room r, Vector3Int pos)
    {
        var mask = r.getShape();
        for (int i = 0; i < mask.GetLength(0); i++)
        {
            for (int j = 0; j < mask.GetLength(1); j++)
            {
                if (mask[i, j] && map[i + pos.x, j + pos.y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
