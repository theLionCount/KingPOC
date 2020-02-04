using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{ 
    void damage(float dmg, float stun, Vector2 dir, float knockback);
}
