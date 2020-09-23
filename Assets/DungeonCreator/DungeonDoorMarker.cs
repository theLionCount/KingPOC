using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorMarker : MonoBehaviour
{
    public Vector2 intoDir;

    public Vector2Int pos;

    public bool open;

    public float corridorChance;

    public void setToWorld(Vector2Int worldPos)
    {
        pos += worldPos;
    }
}
