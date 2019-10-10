using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    float sx, cx;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Rigidbody2D rb2d;
    bool lmb;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        sx = transform.localScale.x;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool sliceV, sliceU, sliceD;
        Vector2 v = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) v.y += 0.19f;
        if (Input.GetKey(KeyCode.A)) v.x -= 0.19f;
        if (Input.GetKey(KeyCode.S)) v.y -= 0.19f;
        if (Input.GetKey(KeyCode.D)) v.x += 0.19f;
        animator.SetBool("MoveV", v.x != 0);
        animator.SetBool("MoveUp", v.y > 0);
        animator.SetBool("MoveDown", v.y < 0);
        if (!lmb && Input.GetMouseButton(0))
        {
            Debug.Log("pressed");
            lmb = true;
            sliceU = v.y > 0;
            sliceD = v.y < 0;
            sliceV = v.x != 0;
            Debug.Log(sliceU);
            Debug.Log(sliceD);
            Debug.Log(sliceV);
            animator.SetBool("SliceUp", sliceU);
            animator.SetBool("SliceDown", sliceD);
            animator.SetBool("SliceV", sliceV);
            animator.SetBool("AnySlice", sliceU || sliceV || sliceD);
        }
        else
        {
            animator.SetBool("SliceUp", false);
            animator.SetBool("SliceDown", false);
            animator.SetBool("SliceV", false);
            animator.SetBool("AnySlice", false);
        }
        if (!Input.GetMouseButton(0)) lmb = false;
        if (v.x < 0) transform.localScale = new Vector3(-sx, transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(sx, transform.localScale.y, transform.localScale.z);
        if (rb2d.Cast(v, contactFilter, hitBuffer, v.magnitude) <= 0) rb2d.position += v;
    }

}
