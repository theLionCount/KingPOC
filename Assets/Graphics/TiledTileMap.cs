using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledTileMap : MonoBehaviour
{
    GameObject player;
    public int sx, sy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int dif = new Vector2Int((int)(player.transform.position.x - transform.position.x) / sx, (int)(player.transform.position.y - transform.position.y) / sy);
        Vector2 t = Vector2.zero;
        if (dif.x > 1) t.x = sx * 3;
        if (dif.x < -1) t.x = -sx * 3;
        if (dif.y > 1) t.x = sy * 3;
        if (dif.y < -1) t.x = -sy * 3;
        transform.position = new Vector3(transform.position.x + t.x, transform.position.y + t.y, transform.position.z);
    }
}
