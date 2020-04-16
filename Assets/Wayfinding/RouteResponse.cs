using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteResponse
{
    public List<Vector2Int> route;
    public bool rdy;
    public bool started;
    public object lck;
    public Vector2Int start;
    public Vector2Int end;

    public RouteResponse()
    {
        route = new List<Vector2Int>();
        lck = new object();
    }
}
