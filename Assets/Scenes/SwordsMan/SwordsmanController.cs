using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordsmanController : MonoBehaviour
{
    Swordsman body;
    long time;
    float countdown;
    bool atTarget;
    float avoidTime;
    MeleeAvoider avoider;
    Vector3 avoidDir;
    MapModule owc;
    RouteProvider routeProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Swordsman>();
        avoider = gameObject.GetComponentInChildren<MeleeAvoider>();
        owc = GameObject.Find("World").GetComponent<MapModule>();
        routeProvider = new RouteProvider();
    }

    Vector3 lastOK;

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 p = GameObject.Find("Player").transform.position;
        if (!atTarget)
        {
            List<Vector3> pss = new List<Vector3>() { p + new Vector3(1.2f, 0, 0), p + new Vector3(-1.2f, 0, 0), p + new Vector3(0, 1.2f, 0), p + new Vector3(0, -1.2f, 0) };
            lastOK = !pss.Exists(v => (v - transform.position).magnitude == pss.Min(t => (t - transform.position).magnitude) && !owc.map[(int)v.x, (int)v.y]) ? lastOK : pss.FirstOrDefault(v => (v - transform.position).magnitude == pss.Min(t => (t - transform.position).magnitude) && !owc.map[(int)v.x, (int)v.y]);
            Vector3 c = lastOK;
            c.z = transform.position.z;
            Vector3 ot = c;
            var route = routeProvider.nextTarget(transform.position, c); //  owc.getRoute(new Vector2Int((int)transform.position.x, (int)transform.position.y), new Vector2Int((int)c.x, (int)c.y));

            if ((c - transform.position).magnitude > 2) c = new Vector3(route.x, route.y, c.z);

            Vector3 dir = (c - transform.position).normalized;



            if (avoider.getAvoidDir().magnitude > 0.1f)
            {
                avoidTime = 10;
                avoidDir = avoider.getAvoidDir();
            }

            if (avoidTime > 0)
            {
                avoidTime--;
                dir += avoidDir;
                dir.Normalize();
            }

            body.move(dir);
            if ((ot - transform.position).magnitude <= 0.4f)
            {
                atTarget = true;
                countdown = 20;
            }
        }
        else
        {
            body.move(Vector2.zero);
            countdown--;
            if (countdown <= 0)
            {
                atTarget = false;

                var t = p - transform.position;
                Vector2 d = Vector2.zero;
                if (Mathf.Abs(t.x) > Mathf.Abs(t.y)) d.x = t.x;
                else d.y = t.y;

                body.slice(d);
            }
        }
    }
}
