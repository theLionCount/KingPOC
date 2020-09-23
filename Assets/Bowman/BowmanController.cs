using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowmanController : MonoBehaviour
{
    CharacterBase body;

    public float reactionTime;
    Vector3 target;
    MeleeAvoider avoider;
    MapModule owc;
    RouteProvider routeProvider;
    public float rld;
    float cooldown;

    protected virtual void Start()
    {
        body = gameObject.GetComponent<CharacterBase>();
        avoider = gameObject.GetComponentInChildren<MeleeAvoider>();
        owc = GameObject.Find("World").GetComponent<MapModule>();
        routeProvider = new RouteProvider();
        target = transform.position + new Vector3(0.1f, 0, 0);
    }

    Vector3 lastOK;

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Vector3 p = GameObject.Find("Player").transform.position;

        if (cooldown > 0) cooldown--;
        bool shot = false;

        if (cooldown <= 0 && PlayerSightlineer.haveSightline(transform.position, 7))
        {
            cooldown = rld;
            body.move(Vector2.zero);
            var t = p - transform.position;
            Vector2 d = Vector2.zero;
            if (Mathf.Abs(t.x) > Mathf.Abs(t.y)) d.x = t.x;
            else d.y = t.y;

            body.slice(d);
            shot = true;
        }
        else
        {
            Vector3 c = target;
            c.z = transform.position.z;
            var route = routeProvider.nextTarget(transform.position, c); 

            if ((c - transform.position).magnitude > 2) c = new Vector3(route.x + 0.5f, route.y + 0.5f, c.z);

            Vector3 dir = (c - transform.position).normalized;

            body.move(dir);
        }


        if ((target - transform.position).magnitude <= 0.3f || shot || !PlayerSightlineer.haveSightline(target, 8))
        {
            target = PlayerSightlineer.getTarget(8, transform.position, true);
        }
    }
}
