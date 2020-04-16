using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSaver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        

        int firstNotNullYM = int.MaxValue;
        for (int z = 0; z < bounds.size.z; z++)
        {
            int firstNotNullY = 0;
            while (firstNotNullY < bounds.size.y)
            {
                bool nulline = true;
                for (int i = 0; i < bounds.size.x; i++)
                {
                    if (tilemap.GetTile(bounds.position + new Vector3Int(i, firstNotNullY, z)) != null) nulline = false;
                }
                if (!nulline) break;
                firstNotNullY++;
            }
            if (firstNotNullY < firstNotNullYM) firstNotNullYM = firstNotNullY;
        }

        int firstNotNullXM = int.MaxValue;
        for (int z = 0; z < bounds.size.z; z++)
        {
            int firstNotNullX = 0;
            while (firstNotNullX < bounds.size.x)
            {
                bool nulline = true;
                for (int i = 0; i < bounds.size.y; i++)
                {
                    if (tilemap.GetTile(bounds.position + new Vector3Int(firstNotNullX, i, z)) != null) nulline = false;
                }
                if (!nulline) break;
                firstNotNullX++;
            }
            if (firstNotNullX < firstNotNullXM) firstNotNullXM = firstNotNullX;
        }

        int lastNotNullYM = int.MinValue;
        for (int z = 0; z < bounds.size.z; z++)
        {
            int lastNotNullY = bounds.size.y - 1;
            while (lastNotNullY >= 0)
            {
                bool nulline = true;
                for (int i = 0; i < bounds.size.x; i++)
                {
                    if (tilemap.GetTile(bounds.position + new Vector3Int(i, lastNotNullY, z)) != null) nulline = false;
                }
                if (!nulline) break;
                lastNotNullY--;
            }
            if (lastNotNullY > lastNotNullYM) lastNotNullYM = lastNotNullY;
        }

        int lastNotNullXM = int.MinValue;
        for (int z = 0; z < bounds.size.z; z++)
        {
            int lastNotNullX = bounds.size.x - 1;
            while (lastNotNullX >= 0)
            {
                bool nulline = true;
                for (int i = 0; i < bounds.size.y; i++)
                {
                    if (tilemap.GetTile(bounds.position + new Vector3Int(lastNotNullX,i, z)) != null) nulline = false;
                }
                if (!nulline) break;
                lastNotNullX--;
            }
            if (lastNotNullX > lastNotNullXM) lastNotNullXM = lastNotNullX;
        }

        lastNotNullXM++;
        lastNotNullYM++;

        bounds.position += new Vector3Int(firstNotNullXM, firstNotNullYM, 0);
        bounds.size -= new Vector3Int(firstNotNullXM + (bounds.size.x - lastNotNullXM), firstNotNullYM + (bounds.size.y - lastNotNullYM), 0);

        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        using (var sw = new StreamWriter("Assets\\TileConstructs\\" + name + ".tile.construct"))
        {
            sw.WriteLine(bounds.size.x.ToString() + " " + bounds.size.y.ToString() + " " + bounds.size.z.ToString());
            foreach (var item in allTiles)
            {
                sw.WriteLine(item == null ? "" : item.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
