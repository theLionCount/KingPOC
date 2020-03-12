using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OutsideCreator : MonoBehaviour
{
    public Tile[] grassToDirtTiles;
    public Tile[] grassToDarkGrassTiles;
    public Tile[] grassToWaterTiles;
    public Tile[] specGlass;
    public Tile[] flowers;
    public Tile[] treeTiles;
    public Tile treeTrunk;
    Tilemap tilemap;
    Tilemap FGTileMap;

    public int flowerNum;

    public GameObject verticalBridge;
    Tilemap verticalBridgeMap;

    public int w, h;
    int ox, oy;
    OutsideWorldCreator owc;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        owc = GameObject.Find("World").GetComponent<OutsideWorldCreator>();
        
        verticalBridgeMap = verticalBridge.GetComponent<Tilemap>();

        tilemap = gameObject.transform.Find("Tilemap").GetComponent<Tilemap>();

        FGTileMap = gameObject.transform.Find("FGTilemap").GetComponent<Tilemap>();


        repaint();

        //for (int i = 0; i < flowerNum; i++)
        //{
        //    Vector2Int treepos;
        //    do { treepos = new Vector2Int(UnityEngine.Random.Range(-w / 2, w / 2), UnityEngine.Random.Range(-h / 2, h / 2)); } while (owc.Roadmap(treepos.x + w / 2, treepos.y + h / 2, ox, oy) || owc.isNextToRoad(treepos.x + w / 2, treepos.y + h / 2, ox, oy) || owc.isOnEdgeOfRoad(treepos.x + w / 2, treepos.y + h / 2, ox, oy));
        //    createTree(treepos.x, treepos.y);
        //   // tilemap.SetTile(new Vector3Int(treepos.x, treepos.y, 0), flowers[0]);
        //    tilemap.SetTile(new Vector3Int(UnityEngine.Random.Range(-w / 2, w / 2), UnityEngine.Random.Range(-h / 2, h / 2), 1), flowers[UnityEngine.Random.Range(0, flowers.Length)]);

        //}

        //foreach (var item in verticalBridgeMap.cellBounds.allPositionsWithin)
        //{

        //} 

    }

    void repaint()
    {
        tilemap.ClearAllTiles();
        FGTileMap.ClearAllTiles();
        ox = (int)transform.position.x;
        oy = (int)transform.position.y;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Tile t = owc.GTDM(i, j, ox, oy) == 0 && UnityEngine.Random.Range(0, 10) == 0 ? specGlass[UnityEngine.Random.Range(0, specGlass.Length)] : grassToDarkGrassTiles[owc.GTDM(i, j, ox, oy)];
                if (owc.Roadmap(i, j, ox, oy)) t = grassToDirtTiles[owc.GTDM(i, j, ox, oy)];
                tilemap.SetTile(new Vector3Int(i, j, 0), t);
            }
        }
    }

    void createTree(int x, int y)
    {
        tilemap.SetTile(new Vector3Int(x, y, 1), treeTrunk);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                FGTileMap.SetTile(new Vector3Int(x - 1 + j, y + 2 - i, h*100-y*100 + x*10 + i), treeTiles[i * 3 + j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool shoudRP = false;
        int ptx = (int)player.transform.position.x / w;
        int pty = (int)player.transform.position.y / h;
        int tx = (int)transform.position.x / w;
        int ty = (int)transform.position.y / h;
        if (ptx + 2 <= tx) { tx = ptx - 1; shoudRP = true; }
        if (ptx - 2 >= tx) { tx = ptx + 1; shoudRP = true; }
        if (pty + 2 <= ty) { ty = pty - 1; shoudRP = true; }
        if (pty - 2 >= ty) { ty = pty + 1; shoudRP = true; }
        if (shoudRP)
        {
            transform.position = new Vector3(tx * w, ty * h, transform.position.z);
            repaint();
        }

    }
}
