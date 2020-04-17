using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : CharacterBase
{
    bool shot;

    public GameObject arrow;
    public string projectileLayer;
    ITargetGettr tg;

    protected override void Start()
    {
        base.Start();
        tg = GetComponent<ITargetGettr>();
    }

    public void fire()
    {
        var blt = Instantiate(arrow, transform.position, new Quaternion());
        blt.layer = LayerMask.NameToLayer(projectileLayer);
        blt.GetComponent<Arrow>().dir = tg.getTarget();
    }

}
