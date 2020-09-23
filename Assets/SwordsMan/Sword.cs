using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public float stun = 20;
    public float kb = 0.14f;
    public float dmg = 0.5f;
    IKillable myIdiot;


   // public Text

    // Start is called before the first frame update
    void Start()
    {
        myIdiot = GetComponentInParent<IKillable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var killable = collision.gameObject.GetComponent<IKillable>();
        if (killable != null) killable.damage(dmg, stun, collision.gameObject.transform.position - gameObject.transform.position, kb, myIdiot);
    }
}
