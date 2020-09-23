using Assets.Scenes.Wayfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapModule : MonoBehaviour
{
    public bool[,] map;
    IMapEnabled owc;
    int w, h;
    List<RouteResponse> responseQueue;
    bool stop;
    object lck;
    Thread processThread;

    // Start is called before the first frame update
    void Start()
    {
        owc = gameObject.GetComponent<IMapEnabled>();
        w = owc.getW();
        h = owc.getH();
        lck = new object();
        map = new bool[w, h];
        responseQueue = new List<RouteResponse>();
        processThread = new Thread(responseProcessing);
        processThread.Priority = System.Threading.ThreadPriority.BelowNormal;
        processThread.Start();
    }

    public List<Vector2Int> getRoute(Vector2 pos, Vector2 target)
    {

        //System.Diagnostics.Debug.WriteLine("getroute Started");
        List<Vector3> nodes = new List<Vector3>();
        List<Vector3> avaible = new List<Vector3>();
        Dictionary<Vector2Int, Vector3> route = new Dictionary<Vector2Int, Vector3>();
        nodes.Add(new Vector3(pos.x, pos.y, 0));
        if (target == pos) return new List<Vector2Int>() { new Vector2Int((int)target.x, (int)target.y), new Vector2Int((int)target.x, (int)target.y) };
        if (map[(int)target.x, (int)target.y])
        {
            //System.Diagnostics.Debug.WriteLine("getroute ended cos target is no good: " + target.x.ToString() + target.y.ToString());
            return null;
        }
        bool rdy = false;
        int count = 0, length = 0;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Restart();
        while (!rdy && count < 400)
        {
            var n = getNext(nodes, target);
            var next = n.Item1;
            route[new Vector2Int((int)next.x, (int)next.y)] = n.Item2;
            next.z -= (int)(Mathf.Sqrt((next.x - target.x) * (next.x - target.x) + (next.y - target.y) * (next.y - target.y)) * 50);
            length = (int)next.z;
            nodes.Add(next);
            if (next.x == target.x && next.y == target.y) rdy = true;
            count++;
        }
        //System.Diagnostics.Debug.WriteLine("getRoute ended: " + count.ToString());
        sw.Stop(); //System.Diagnostics.Debug.WriteLine("getRoute length = " + sw.Elapsed.Milliseconds.ToString());
        if (rdy)
        {
            List<Vector2Int> ret = new List<Vector2Int>();
            var current = target;
            while (true)
            {
                ret.Insert(0, new Vector2Int((int)current.x, (int)current.y));
                if (ret[0].x == (int)pos.x && ret[0].y == (int)pos.y) return ret;
                current = new Vector2Int((int)route[new Vector2Int((int)current.x, (int)current.y)].x, (int)route[new Vector2Int((int)current.x, (int)current.y)].y);
                
            }
        }

        return null;
    }

    Tuple<Vector3, Vector3> getNext(List<Vector3> nodes, Vector2 target)
    {
        Tuple<Vector3, Vector3> r = new Tuple<Vector3, Vector3>(new Vector3(0, 0, int.MaxValue), Vector3.zero);
        foreach (var item in nodes)
        {
            int x = (int)item.x;
            int y = (int)item.y;
            Vector3 ret = new Vector3(0, 0, int.MaxValue);
            if (x > 0 && y > 0 && !map[x - 1, y - 1] && !nodes.Exists(t => t.x == x - 1 && t.y == y - 1)) ret = addToNodeList(ret, createNode(item, target, x - 1, y - 1));
            if (x > 0 && !map[x - 1, y] && !nodes.Exists(t => t.x == x - 1 && t.y == y)) ret = addToNodeList(ret, createNode(item, target, x - 1, y));
            if (x > 0 && y < h && !map[x - 1, y + 1] && !nodes.Exists(t => t.x == x - 1 && t.y == y + 1)) ret = addToNodeList(ret, createNode(item, target, x - 1, y + 1));

            if (y > 0 && !map[x, y - 1] && !nodes.Exists(t => t.x == x && t.y == y - 1)) ret = addToNodeList(ret, createNode(item, target, x, y - 1));

            if (y < h && !map[x, y + 1] && !nodes.Exists(t => t.x == x && t.y == y + 1)) ret = addToNodeList(ret, createNode(item, target, x, y + 1));

            if (x < w && y > 0 && !map[x + 1, y - 1] && !nodes.Exists(t => t.x == x + 1 && t.y == y - 1)) ret = addToNodeList(ret, createNode(item, target, x + 1, y - 1));
            if (x < w && !map[x + 1, y] && !nodes.Exists(t => t.x == x + 1 && t.y == y)) ret = addToNodeList(ret, createNode(item, target, x + 1, y));
            if (x < w && y < h && !map[x + 1, y + 1] && !nodes.Exists(t => t.x == x + 1 && t.y == y + 1)) ret = addToNodeList(ret, createNode(item, target, x + 1, y + 1));
            if (ret.z < r.Item1.z) r = new Tuple<Vector3, Vector3>(ret, item);
        }
        return r;
    }

    Vector3 addToNodeList(Vector3 current, Vector3 newNode)
    {
        return current.z < newNode.z ? current : newNode;
    }

    Vector3 createNode(Vector3 item, Vector2 target, int x, int y) => new Vector3(x, y, (float)(item.z + 1 + Mathf.Sqrt((x - target.x) * (x - target.x) + (y - target.y) * (y - target.y)) * 50));

    public void queRouteRequest(RouteResponse response)
    {
        if (!responseQueue.Contains(response)) 
        {
            //if (response.route == null)
                //System.Diagnostics.Debug.WriteLine("wtf is happening right now?");
            response.rdy = false;
            lock (lck) responseQueue.Add(response); 
        }
    }

    void responseProcessing()
    {
        while (!stop)
        {
            RouteResponse current;

            if (responseQueue.Count <= 0)
            {
                Thread.Sleep(20);
            }
            else
            {

                lock (lck)
                {
                    current = responseQueue[0];
                    responseQueue.RemoveAt(0);
                }

                lock (current.lck)
                {
                    current.started = true;
                    current.rdy = false;
                    //System.Diagnostics.Debug.WriteLine("route locked");
                }
                //System.Diagnostics.Debug.WriteLine("modifying route from processing");
                current.route = getRoute(new Vector2Int((int)current.start.x,(int)current.start.y), new Vector2Int((int)current.end.x, (int)current.end.y));
                //System.Diagnostics.Debug.WriteLine("modifyied route from processing");
                //if (current.route[0] != current.start)
                //{
                //    //System.Diagnostics.Debug.WriteLine("wut?");
                //}
                lock (current.lck)
                {
                    current.started = false;
                    current.rdy = true;
                    //System.Diagnostics.Debug.WriteLine("route open");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        stop = gameObject == null;
    }
}
