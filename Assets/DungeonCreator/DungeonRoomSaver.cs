using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoomSaver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tm = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        var fg = GameObject.Find("Foreground").GetComponent<Tilemap>();
        var ffg = GameObject.Find("FullFG").GetComponent<Tilemap>();
        foreach (var item in GameObject.FindGameObjectsWithTag("DungeonRoom"))
        {
            using (var sw = new StreamWriter(item.name + ".room"))
            {
                var dr = item.GetComponent<DungeonRoom>();
                sw.WriteLine(dr.bottomLeft);
                sw.WriteLine(dr.topRight);
                for (int i = (int)dr.bottomLeft.x; i < (int)dr.topRight.x; i++)
                {
                    for (int j = (int)dr.bottomLeft.y; j < (int)dr.topRight.y; j++)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            sw.WriteLine(tm.GetTile(new Vector3Int(i, j, k))?.name);
                            sw.WriteLine(fg.GetTile(new Vector3Int(i, j, k))?.name);
                            sw.WriteLine(ffg.GetTile(new Vector3Int(i, j, k))?.name);
                        }
                    }
                }
            }
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("Corridor"))
        {
            using (var sw = new StreamWriter(item.name + ".corridor"))
            {
                var dr = item.GetComponent<DungeonRoom>();
                sw.WriteLine(dr.bottomLeft);
                sw.WriteLine(dr.topRight);
                for (int i = (int)dr.bottomLeft.x; i < (int)dr.topRight.x; i++)
                {
                    for (int j = (int)dr.bottomLeft.y; j < (int)dr.topRight.y; j++)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            sw.WriteLine(tm.GetTile(new Vector3Int(i, j, k))?.name);
                            sw.WriteLine(fg.GetTile(new Vector3Int(i, j, k))?.name);
                            sw.WriteLine(ffg.GetTile(new Vector3Int(i, j, k))?.name);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
