using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterBase body;
    bool lmb;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<CharacterBase>();
    }

    void FixedUpdate()
    {
        var v = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) v.y = 1f;
        if (Input.GetKey(KeyCode.A)) v.x = -1f;
        if (Input.GetKey(KeyCode.S)) v.y = -1f;
        if (Input.GetKey(KeyCode.D)) v.x = 1f;
        body.move(v);
        if (!lmb && Input.GetMouseButton(0))
        {
            lmb = true;
            var d = Vector2.zero;
            var t = Input.mousePosition;
            t.x -= Screen.width / 2;
            t.y -= Screen.height / 2;

            if (Mathf.Abs(t.x) > Mathf.Abs(t.y)) d.x = t.x;
            else d.y = t.y;

            body.slice(d);
        }
        if (!Input.GetMouseButton(0))
        {
            lmb = false;
        }
        body.shield(Input.GetMouseButton(1));
    }
}
