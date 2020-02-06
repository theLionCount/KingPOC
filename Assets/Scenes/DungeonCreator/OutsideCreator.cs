using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OutsideCreator : MonoBehaviour
{
    public Tile[] grassToDirtTiles;
    public Tile[] grassToDarkGrassTiles;
    public Tile[] specGlass;
    public Tile[] flowers;
    Tilemap tilemap;

    public int w, h;
    public int flowerNum;

    byte[,] gtdm;

    // Start is called before the first frame update
    void Start()
    {
        gtdm = new byte[w, h];
        roadmap = new bool[w, h];
        tilemap = GetComponent<Tilemap>();
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                byte current;
                do
                    current = (byte)UnityEngine.Random.Range(0, 16);
                while (!isByteOk(i, j, current));
            
                gtdm[i, j] = current;
            }
        }

        for (int i = 0; i < flowerNum; i++)
        {
            tilemap.SetTile(new Vector3Int(UnityEngine.Random.Range(-w / 2, w / 2), UnityEngine.Random.Range(-h / 2, h / 2), 1), flowers[UnityEngine.Random.Range(0, flowers.Length)]);
        }

        createRoad();

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Tile t = gtdm[i, j] == 0 && UnityEngine.Random.Range(0, 10) == 0 ? specGlass[UnityEngine.Random.Range(0, specGlass.Length)] : grassToDarkGrassTiles[gtdm[i, j]];
                if (roadmap[i, j]) t = grassToDirtTiles[gtdm[i, j]];
                tilemap.SetTile(new Vector3Int(i - w / 2, j - h / 2, 0), t);
            }
        }
        
    }

    bool[,] roadmap;
    void createRoad()
    {
        Vector2Int[] nodes = new Vector2Int[] { new Vector2Int(0, UnityEngine.Random.Range(10, h-10)), new Vector2Int(w / 3, UnityEngine.Random.Range(10, h-10)), new Vector2Int(w / 3 * 2, UnityEngine.Random.Range(10, h-10)), new Vector2Int(w-10, UnityEngine.Random.Range(10, h-10)) };
        
        for (int i = 0; i < 3; i++)
        {
            for (int x = nodes[i].x; x < nodes[i+1].x; x++)
            {
                int xrelative = x - nodes[i].x;
                int ymax = nodes[i + 1].y - nodes[i].y;
                int xmax = nodes[i + 1].x - nodes[i].x;
                int yrelative = (int)(ymax * ((float)xrelative / xmax));
                int y = nodes[i].y + yrelative;
                for (int ri = 0; ri < 5; ri++)
                {
                    for (int rj = 0; rj < 5; rj++)
                    {
                        roadmap[x + ri, y + rj] = true;
                        gtdm[x + ri, y + rj] = 0;
                    }
                }
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (isNextToRoad(i,j))
                {
                    byte current;
                    do
                        current = (byte)UnityEngine.Random.Range(0, 16);
                    while (!isByteOkFullRoadEdge(i, j, current, isNextToRoad));
                    gtdm[i, j] = current;
                }
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (roadmap[i, j]) gtdm[i, j] = 15;
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (isOnEdgeOfRoad(i, j))
                {
                    byte current;
                    do
                        current = (byte)UnityEngine.Random.Range(0, 16);
                    while (!isByteOkFullRoadEdge(i, j, current, isOnEdgeOfRoad));
                    gtdm[i, j] = current;
                }
            }
        }
    }

    bool isNextToRoad(int i, int j)
    {
        if (roadmap[i, j]) return false;
        if (i > 0 && roadmap[i - 1, j]) return true;
        if (i < w - 1 && roadmap[i + 1, j]) return true;
        if (j > 0 && roadmap[i, j - 1]) return true;
        if (j < h - 1 && roadmap[i, j + 1]) return true;
        if (i > 0 && j > 0 && roadmap[i - 1, j - 1]) return true;
        if (i > 0 && j < h - 1 && roadmap[i - 1, j + 1]) return true;
        if (i < w - 1 && j > 0 && roadmap[i + 1, j - 1]) return true;
        if (i < w - 1 && j < h - 1 && roadmap[i + 1, j + 1]) return true;

        return false;
    }

    bool isOnEdgeOfRoad(int i, int j)
    {
        if (!roadmap[i, j]) return false;
        if (i > 0 && !roadmap[i - 1, j]) return true;
        if (i < w - 1 && !roadmap[i + 1, j]) return true;
        if (j > 0 && !roadmap[i, j - 1]) return true;
        if (j < h - 1 && !roadmap[i, j + 1]) return true;
        if (i > 0 && j > 0 && !roadmap[i - 1, j - 1]) return true;
        if (i > 0 && j < h - 1 && !roadmap[i - 1, j + 1]) return true;
        if (i < w - 1 && j > 0 && !roadmap[i + 1, j - 1]) return true;
        if (i < w - 1 && j < h - 1 && !roadmap[i + 1, j + 1]) return true;

        return false;
    }

    bool isByteOk(int i, int j, byte b)
    {
        if (i > 0 && !(((b & 4) == 0) == ((gtdm[i - 1, j] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i - 1, j] & 1) == 0))) return false;

        if (j > 0 && !(((b & 2) == 0) == ((gtdm[i, j - 1] & 1) == 0) && ((b & 4) == 0) == ((gtdm[i, j - 1] & 8) == 0))) return false;

        return true;
    }

    bool isByteOkFull(int i, int j, byte b)
    {
        if (i > 0 && !(((b & 4) == 0) == ((gtdm[i - 1, j] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i - 1, j] & 1) == 0))) return false;

        if (i < w - 1 && !(((b & 2) == 0) == ((gtdm[i + 1, j] & 4) == 0) && ((b & 1) == 0) == ((gtdm[i + 1, j] & 8) == 0))) return false;

        if (j > 0 && !(((b & 2) == 0) == ((gtdm[i, j - 1] & 1) == 0) && ((b & 4) == 0) == ((gtdm[i, j - 1] & 8) == 0))) return false;

        if (j < h - 1 && !(((b & 1) == 0) == ((gtdm[i, j + 1] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i, j + 1] & 4) == 0))) return false;

        if (i > 0 && j > 0 && !(((b & 8) == 0) == ((gtdm[i - 1, j - 1] & 2) == 0))) return false;

        if (i > 0 && j < h - 1 && !(((b & 4) == 0) == ((gtdm[i - 1, j + 1] & 1) == 0))) return false;

        if (i < w - 1 && j > 0 && !(((b & 1) == 0) == ((gtdm[i + 1, j - 1] & 4) == 0))) return false;

        if (i < w - 1 && j < h - 1 && !(((b & 2) == 0) == ((gtdm[i + 1, j + 1] & 8) == 0))) return false;

        return true;
    }

    bool isByteOkFullRoadEdge(int i, int j, byte b, Func<int,int, bool> edgeCheck)
    {
        if (i > 0 && !edgeCheck(i - 1, j) && !(((b & 4) == 0) == ((gtdm[i - 1, j] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i - 1, j] & 1) == 0))) return false;

        if (i < w - 1 && !edgeCheck(i + 1, j) && !(((b & 2) == 0) == ((gtdm[i + 1, j] & 4) == 0) && ((b & 1) == 0) == ((gtdm[i + 1, j] & 8) == 0))) return false;

        if (j > 0 && !edgeCheck(i, j - 1) && !(((b & 2) == 0) == ((gtdm[i, j - 1] & 1) == 0) && ((b & 4) == 0) == ((gtdm[i, j - 1] & 8) == 0))) return false;

        if (j < h - 1 && !edgeCheck(i, j + 1) && !(((b & 1) == 0) == ((gtdm[i, j + 1] & 2) == 0) && ((b & 8) == 0) == ((gtdm[i, j + 1] & 4) == 0))) return false;

        if (i > 0 && j > 0 && !edgeCheck(i - 1, j - 1) && !(((b & 4) == 0) == ((gtdm[i - 1, j - 1] & 1) == 0))) return false;

        if (i > 0 && j < h - 1 && !edgeCheck(i - 1, j + 1) && !(((b & 8) == 0) == ((gtdm[i - 1, j + 1] & 2) == 0))) return false;

        if (i < w - 1 && j > 0 && !edgeCheck(i + 1, j - 1) && !(((b & 2) == 0) == ((gtdm[i + 1, j - 1] & 8) == 0))) return false;

        if (i < w - 1 && j < h - 1 && !edgeCheck(i + 1, j + 1) && !(((b & 1) == 0) == ((gtdm[i + 1, j + 1] & 4) == 0))) return false;

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
