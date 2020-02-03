using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : MonoBehaviour, IKillable
{
    Animator animator;
    float sx, cx;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Rigidbody2D rb2d;
    bool lmb;
    bool mirrored;
    bool sliceing;
    Vector2 v = Vector2.zero;

    public float speed;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(/*Physics2D.GetLayerCollisionMask(gameObject.layer) & */~Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Walls")));
        contactFilter.useLayerMask = true;
        sx = transform.localScale.x;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("slice"))
        {
            movement();
            animator.SetBool("Recovered", true);
        }
        else
            animator.SetBool("Recovered", false);
        direction();
        if (sliceing) sliceing = false;
        else setNoSlice();
    }

    public void slice(Vector2 dir)
    {
        if (dir.x != 0)
        {
            animator.SetBool("SliceV", true);
            mirrored = dir.x < 0;
        }
        else if (dir.y > 0) animator.SetBool("SliceUp", true);
        else animator.SetBool("SliceDown", true);
        sliceing = true;
    }

    public void setNoSlice()
    {
        animator.SetBool("SliceUp", false);
        animator.SetBool("SliceDown", false);
        animator.SetBool("SliceV", false);
    }

    public void move(Vector2 dir)
    {
        v = dir;
    }

    void direction()
    {
        if (mirrored) transform.localScale = new Vector3(-sx, transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(sx, transform.localScale.y, transform.localScale.z);
    }

    void movement()
    {
        v.Normalize();

        animator.SetBool("MoveV", v.x != 0);
        animator.SetBool("MoveUp", v.y > 0);
        animator.SetBool("MoveDown", v.y < 0);

        mirrored = v.x < 0;

        if (rb2d.Cast(v, contactFilter, hitBuffer, v.magnitude * speed * 5f) <= 0) rb2d.position += v * speed;
    }

    public void damage(float dmg)
    {
        
    }
}
