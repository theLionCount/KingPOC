using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowmanTargetGetter : MonoBehaviour, ITargetGettr
{
    GameObject player;

    public Vector2 getTarget()
    {
        return (player.transform.position - transform.position).normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
