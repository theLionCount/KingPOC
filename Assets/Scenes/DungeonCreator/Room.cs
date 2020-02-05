using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public int width;
    public int height;
    public int roundiness;

    public Room()
    {
        width = Random.Range(10, 50);
        height = Random.Range(10, 50);

        roundiness = Random.Range(0, Mathf.Max(width,height)/3);

    }

    public bool[,] getShape()
    {
        bool[,] ret = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i + j >= roundiness) ret[i, j] = true;
            }
        }
        return ret;
    }

    public void drawRoom(Tilemap map, Tile wall, Tile floor, Vector3Int pos)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i + j >= roundiness)
                {
                    if (i + j == roundiness || i == 0 || i == width - 1 || j == 0 || j == roundiness) map.SetTile(new Vector3Int(i,j,0) + pos, wall);
                    else map.SetTile(new Vector3Int(i, j, 0) + pos, floor);
                }
            }
        }

        for (int i = width / 2 - 1; i <= width / 2 + 1; i++)
        {
            map.SetTile(new Vector3Int(i, 0, 0) + pos, floor);
            map.SetTile(new Vector3Int(i, height-1, 0) + pos, floor);
        }
        for (int i = height / 2 - 1; i <= height / 2 + 1; i++)
        {
            map.SetTile(new Vector3Int(0, i, 0) + pos, floor);
            map.SetTile(new Vector3Int(0, width-1, 0) + pos, floor);
        }

    }
}
