using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSightlineer : MonoBehaviour
{
    static bool[,] sightlines;
    GameObject player;
    ContactFilter2D contactFilter;
    RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    Rigidbody2D rb2d;

    public static int w = 20, h = 20;
    static int x, y;

    // Start is called before the first frame update
    void Start()
    {
        sightlines = new bool[w, h];
        player = GameObject.Find("Player");
        rb2d = player.GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(LayerMask.GetMask("Walls"));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = (int)player.transform.position.x;
        y = (int)player.transform.position.y;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                int xv = x + i - w / 2;
                int yv = y + j - w / 2;
                var cast = new Vector2(xv - x, yv - y);
                if (rb2d.Cast(cast, contactFilter, hitBuffer, cast.magnitude) <= 0) sightlines[i, j] = true;
                else sightlines[i, j] = false;
            }
        }
    }

    public static Vector2 getTarget(int r, Vector2 pos, bool addRamdomness = false)
    {
        var player = new Vector2Int(w/2, h/2);
        var p = new Vector2Int((int)pos.x - x + w / 2, (int)pos.y - y + h / 2);
        float min = float.MaxValue;
        Vector2 tgt = p;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (sightlines[i, j] && new Vector2Int(i - player.x, j - player.y).magnitude <= r && new Vector2Int(i - p.x, j - p.y).magnitude < min)
                {
                    min = new Vector2Int(i - p.x, j - p.y).magnitude;
                    tgt = new Vector2(i, j);
                }
            }
        }
        if (addRamdomness)
        {
            Vector2 rv = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            while (!sightlines[(int)tgt.x + (int)rv.x, (int)tgt.y + (int)rv.y] && new Vector2Int((int)tgt.x + (int)rv.x - player.x, (int)tgt.y + (int)rv.y - player.y).magnitude <= r) rv = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            tgt += rv;
        }
        return tgt + new Vector2(x - w / 2, y - h / 2);
    }

    public static bool haveSightline(Vector2 pos, int r)
    {
        var p = new Vector2Int((int)pos.x - x + w / 2, (int)pos.y - y + h / 2);
        return (p.x >= 0 && p.x < w && p.y >= 0 && p.y < h && new Vector2Int(p.x - w / 2, p.y - h / 2).magnitude <= r) ? sightlines[p.x, p.y] : false;
    }
}
