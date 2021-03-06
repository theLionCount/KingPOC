﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IKillable
{
    protected Animator animator;
    float sx, cx;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Rigidbody2D rb2d;
    bool mirrored;
    bool sliceing;
    Vector2 v = Vector2.zero;

    public float speed;
    protected float stunned;
    Vector2 knockbackV;
    public float friction = 0.9f;
    public float minKBV = 0.005f;

    public float health;

    Color defaultColor;
    SpriteRenderer renderer;

    bool shielding;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(~(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Walls")) | Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("MeleeCollider"))));
        contactFilter.useLayerMask = true;
        sx = transform.localScale.x;
        animator = GetComponent<Animator>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = renderer.color;
    }

    // Update is called once per frame

    protected virtual void FixedUpdate()
    {
        if (stunned > 0)
        {
            stunned--;
            renderer.color = (int)(stunned) % 2 == 0 ? defaultColor : Color.gray;
            animator.SetBool("Stunned", true);
            if (knockbackV.magnitude > minKBV) knockbackV *= friction;
            else knockbackV = Vector2.zero;
            collidedMove(knockbackV);
        }
        else
        {
            
            renderer.material.color = defaultColor;
            animator.SetBool("Stunned", false);
            if (health > 0)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("slice") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("shield")) movement();

                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("slice"))
                {
                    animator.SetBool("Recovered", true);
                }
                else
                    animator.SetBool("Recovered", false);
                if (sliceing) sliceing = false;
                else setNoSlice();

                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("shield"))
                {
                    animator.SetBool("ShieldRecovered", true);
                }
                else
                    animator.SetBool("ShieldRecovered", false);
                animator.SetBool("Shield", shielding);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        direction();
    }

    public void slice(Vector2 dir)
    {
        if (stunned <= 0)
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
    }

    public void setNoSlice()
    {
        animator.SetBool("SliceUp", false);
        animator.SetBool("SliceDown", false);
        animator.SetBool("SliceV", false);
    }

    public void move(Vector2 dir)
    {
        v = shielding ? Vector2.zero : dir;
    }

    public void shield(bool s)
    {
        shielding = s;
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
        if (v.x != 0 && !sliceing) mirrored = v.x < 0;

        collidedMove(v * speed);
    }

    public void damage(float dmg, float stun, Vector2 dir, float knockback, IKillable idiot)
    {
        if (shielding)
            dir.GetType();

        if (!shielding)
        {
            stunned = stun;
            knockbackV = dir.normalized * knockback;
            health -= dmg;
        }
        else if (idiot != null)
        {
            idiot.damage(0, stun / 1.2f, -dir, knockback, this);

        }
    }

    public void collidedMove(Vector2 to)
    {
        Vector2 horizontal = new Vector2(to.x, 0);
        Vector2 vertical = new Vector2(0, to.y);

        float magnitudeAdjustment = rb2d.Cast(horizontal, contactFilter, hitBuffer, 0) > 0 ? 3f : 2f;


        if (rb2d.Cast(horizontal, contactFilter, hitBuffer, horizontal.magnitude * magnitudeAdjustment) <= 0) rb2d.position += horizontal;
        if (rb2d.Cast(vertical, contactFilter, hitBuffer, vertical.magnitude * magnitudeAdjustment) <= 0) rb2d.position += vertical;
    }
}