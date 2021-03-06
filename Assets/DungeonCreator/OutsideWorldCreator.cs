﻿using Assets.Scenes.Wayfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class OutsideWorldCreator : MonoBehaviour, IMapEnabled
{
    byte[,] gtdm;
    public int w, h;
    public int flowerNum;
    public int tileW, tileH;
    List<Vector2Int>[,] trees;
    MapModule mapModule;
    Dictionary<string, TilemapLoader> loaders = new Dictionary<string, TilemapLoader>();
    GradientCraetor gc;

    public List<Tuple<Vector3Int, TilemapLoader>>[,] constructs;

    public byte GTDM(int x, int y, int ox, int oy)
    {
        if (x + ox < 0 || x + ox >= w || y + oy < 0 || y + oy >= h) return 0;
        return gtdm[x + ox, y + oy];
    }

    // Start is called before the first frame update
    void Start()
    {
        mapModule = gameObject.GetComponent<MapModule>();
        roadmap = new bool[w, h];
        trees = new List<Vector2Int>[w / tileW, h / tileH];
        constructs = new List<Tuple<Vector3Int, TilemapLoader>>[w / tileW, h / tileH];
        loadConstructs();
        for (int i = 0; i < w / tileW; i++)
        {
            for (int j = 0; j < h / tileH; j++)
            {
                trees[i, j] = new List<Vector2Int>();
            }
        }
        gc = new GradientCraetor(w, h);
        gtdm = gc.gtdm;

        //createRoad(10);

        createLakes();
        //for (int i = 0; i < 20; i++)
        //{
        //    createRoad(20);
        //}
        smoothenRoad();

        createTrees();
        //constructs[60 / tileW, 40 / tileH] = new List<Tuple<Vector3Int, TilemapLoader>>();
        //constructs[60 / tileW, 40 / tileH].Add(new Tuple<Vector3Int, TilemapLoader>(new Vector3Int(60, 40, 1), loaders["SmallHouse"]));
    }

    void loadConstructs()
    {
        foreach (var item in Directory.GetFiles("Assets\\TileConstructs","*.tile.construct"))
        {
           // loaders[item.Split('\\').Last().Split('.')[0]] = new TilemapLoader(item);
        }
    }

    void createTrees()
    {
        for (int i = 0; i < flowerNum; i++)
        {
            Vector2Int treepos;
            do { treepos = new Vector2Int(UnityEngine.Random.Range(1, w - 1), UnityEngine.Random.Range(1, h - 1)); } while (roadmap[treepos.x, treepos.y] || isNextToRoad(treepos.x, treepos.y) || isOnEdgeOfRoad(treepos.x, treepos.y) || (!isForest(treepos.x,treepos.y) && !(UnityEngine.Random.Range(0,20) == 0)));
          
            trees[treepos.x / tileW, treepos.y / tileH].Add(treepos);
            mapModule.map[treepos.x - 1, treepos.y - 1] = true;
            mapModule.map[treepos.x - 1, treepos.y] = true;
            mapModule.map[treepos.x - 1, treepos.y + 1] = true;
            mapModule.map[treepos.x, treepos.y - 1] = true;
            mapModule.map[treepos.x, treepos.y] = true;
            mapModule.map[treepos.x, treepos.y + 1] = true;
            mapModule.map[treepos.x + 1, treepos.y - 1] = true;
            mapModule.map[treepos.x + 1, treepos.y] = true;
            mapModule.map[treepos.x + 1, treepos.y + 1] = true;

            //do { treepos = new Vector2Int(UnityEngine.Random.Range(1, w - 1), UnityEngine.Random.Range(1, h - 1)); } while (roadmap[treepos.x, treepos.y] || isNextToRoad(treepos.x, treepos.y) || isOnEdgeOfRoad(treepos.x, treepos.y));
            //constructs.Add(new Tuple<Vector3Int, TilemapLoader>(new Vector3Int(treepos.x, treepos.y, 1), loaders["HouseBeta"]));
        }
    }

    
    

    public List<Vector2Int> getTrees(int ox, int oy) => trees[ox / tileW, oy / tileH];

    public List<Tuple<Vector3Int, TilemapLoader>> getConstructs(int ox, int oy) => constructs[ox / tileW, oy / tileH];

    // Update is called once per frame
    void Update()
    {

    }




    List<Vector2> CatmulRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int resolution, float alpha)
    {
        List<Vector2> ret = new List<Vector2>();

        float t0 = 0.0f;
        float t1 = GetT(t0, p0, p1, alpha);
        float t2 = GetT(t1, p1, p2, alpha);
        float t3 = GetT(t2, p2, p3, alpha);

        for (float t = t1; t < t2; t += ((t2 - t1) / (float)resolution))
        {
            Vector2 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector2 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector2 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector2 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector2 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector2 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            ret.Add(C);
        }
        return ret;
    }

    float GetT(float t, Vector2 p0, Vector2 p1, float alpha)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, alpha);

        return (c + t);
    }

    bool[,] roadmap;

    public bool Roadmap(int x, int y, int ox, int oy)
    {
        if (x + ox < 0 || x + ox >= w || y + oy < 0 || y + oy >= h) return false;
        return roadmap[x + ox, y + oy];
    }

    bool isLake(int i, int j) => Mathf.PerlinNoise(i / 100f, j / 100f) + Mathf.PerlinNoise(i / 100f + 40, j / 100f) > 1.4;
    bool isForest(int i, int j) => Mathf.PerlinNoise(i / 250f, j / 250f + 40) > 0.637f;

    void createRoad(int nodenum)
    {
        Vector2Int[] nodes = new Vector2Int[nodenum];
        nodes[0] = new Vector2Int(0, UnityEngine.Random.Range(10, h - 10));
        for (int i = 1; i < nodenum; i++)
        {
            do
            {
                nodes[i] = new Vector2Int(i * w / nodenum - 10, nodes[i - 1].y + UnityEngine.Random.Range(-h / 6, h / 6));
            } while (nodes[i].y < 10 || nodes[i].y > w - 10);
        }

        List<Vector2> points = new List<Vector2>();
        for (int i = 3; i < nodenum; i++)
        {
            points.AddRange(CatmulRom(nodes[i - 3], nodes[i - 2], nodes[i - 1], nodes[i], 60, 0.4f));
        }


        for (int i = 0; i < points.Count - 1; i++)
        {
            for (int x = (int)points[i].x; x < points[i + 1].x; x++)
            {
                int xrelative = x - (int)points[i].x;
                int ymax = (int)points[i + 1].y - (int)points[i].y;
                int xmax = (int)points[i + 1].x - (int)points[i].x;
                int yrelative = (int)(ymax * ((float)xrelative / xmax));
                int y = (int)points[i].y + yrelative;
                for (int ri = 0; ri < 5; ri++)
                {
                    for (int rj = 0; rj < 5; rj++)
                    {
                        if (x + ri >= 0 && x + ri < w && y + rj >= 0 && y + rj < h)
                        {
                            roadmap[x + ri, y + rj] = true;
                            gtdm[x + ri, y + rj] = 0;
                        }
                    }
                }
            }
        }
    }

    void createLakes()
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (isLake(i,j))
                {
                    roadmap[i, j] = true;
                    gtdm[i, j] = 0;
                }
            }
        }

    }

    void smoothenRoad()
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (isNextToRoad(i, j))
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

    public bool isNextToRoad(int i, int j, int ox, int oy) => isNextToRoad(i + ox, j + oy);

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

    public bool isOnEdgeOfRoad(int i, int j, int ox, int oy) => isOnEdgeOfRoad(i + ox, j + oy);

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

    bool isByteOkFullRoadEdge(int i, int j, byte b, Func<int, int, bool> edgeCheck)
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

    public int getW()
    {
        return w;
    }

    public int getH()
    {
        return h;
    }
}
