using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAvoider : MonoBehaviour
{
    List<GameObject> close = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        close.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        close.Remove(collision.gameObject);
    }

    public Vector2 getAvoidDir()
    {
        if (close.Count == 0) return Vector2.zero;
        var dir = Vector3.zero;
        foreach (var item in close)
        {
            if ((item.transform.position - transform.position).magnitude < 0.01f)
                item.transform.position += new Vector3((Random.value - 0.5f) * 0.1f, (Random.value - 0.5f) * 0.1f, 0);
            dir += (transform.position - item.transform.position).normalized;
        }
        return dir.normalized;
    }
}
