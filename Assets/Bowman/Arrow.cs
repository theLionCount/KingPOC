using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    Vector3 v;


    // Start is called before the first frame update
    void Start()
    {
        v = speed * dir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += v;
        transform.rotation = Quaternion.Euler(0, 0, v.x > 0 ? -(float)Vector2.Angle(new Vector2(0, 1), v) : (float)Vector2.Angle(new Vector2(0, 1), v));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var killable = collision.gameObject.GetComponent<IKillable>();
        if (killable != null) killable.damage(0.5f, 15, v, 0.12f);
        Destroy(gameObject);
    }
}
