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
    
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Swordsman>();
        avoider = gameObject.GetComponentInChildren<MeleeAvoider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 p = GameObject.Find("Player").transform.position;
        if (!atTarget)
        {
            List<Vector3> pss = new List<Vector3>() { p + new Vector3(1.2f, 0, 0), p + new Vector3(-1.2f, 0, 0), p + new Vector3(0, 1.2f, 0), p + new Vector3(0, -1.2f, 0) };
            Vector3 c = pss.First(v => (v - transform.position).magnitude == pss.Min(t => (t - transform.position).magnitude));


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
            if ((c - transform.position).magnitude <= 0.4f)
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
