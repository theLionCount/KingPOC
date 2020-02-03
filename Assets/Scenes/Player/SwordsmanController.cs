using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanController : MonoBehaviour
{
    Swordsman body;
    long time;
    
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Swordsman>();
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        if ((time / 30) % 2 == 0) body.move(new Vector2(1, 0));
        else body.move(new Vector2(-1, 0));
        if (Random.Range(0, 100) == 0) body.slice(new Vector2(1, 0));
        //else body.setNoSlice();
    }
}
