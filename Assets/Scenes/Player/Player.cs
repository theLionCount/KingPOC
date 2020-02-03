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

    bool mirrored;

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
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("slice")) move();
        else animator.SetBool("Recovered", false);
        sliceControll();
        if (mirrored) transform.localScale = new Vector3(-sx, transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(sx, transform.localScale.y, transform.localScale.z);

    }

    void move()
    {
        Vector2 v = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) v.y += 0.19f;
        if (Input.GetKey(KeyCode.A)) v.x -= 0.19f;
        if (Input.GetKey(KeyCode.S)) v.y -= 0.19f;
        if (Input.GetKey(KeyCode.D)) v.x += 0.19f;
        animator.SetBool("MoveV", v.x != 0);
        animator.SetBool("MoveUp", v.y > 0);
        animator.SetBool("MoveDown", v.y < 0);
        mirrored = v.x < 0;
        if (rb2d.Cast(v, contactFilter, hitBuffer, v.magnitude) <= 0) rb2d.position += v;
    }

    void sliceControll()
    {
        bool sliceV, sliceU, sliceD;
       
        if (!lmb && Input.GetMouseButton(0))
        {
            var t = Input.mousePosition;
            t.x -= Screen.width / 2;
            t.y -= Screen.height / 2;

            sliceD = false;
            sliceU = false;
            sliceV = false;

            if (Mathf.Abs(t.x) > Mathf.Abs(t.y))
            {
                sliceV = true;
                mirrored = t.x < 0;
            }
            else if (t.y > 0) sliceU = true;
            else sliceD = true;

            lmb = true;
            animator.SetBool("SliceUp", sliceU);
            animator.SetBool("SliceDown", sliceD);
            animator.SetBool("SliceV", sliceV);
        }
        if (!Input.GetMouseButton(0))
        {
            lmb = false;
            animator.SetBool("SliceUp", false);
            animator.SetBool("SliceDown", false);
            animator.SetBool("SliceV", false);
            animator.SetBool("AnySlice", false);
            animator.SetBool("Recovered", true);
        }
    }
    
}
