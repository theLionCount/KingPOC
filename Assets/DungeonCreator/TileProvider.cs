using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    // Start is called before the first frame update
    void Start()
    {
        var at = Resources.FindObjectsOfTypeAll<Tile>();
        foreach (var item in at)
        {
            tiles[item.name] = item;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
