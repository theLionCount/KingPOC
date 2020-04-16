using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    GameObject light;
    Vector3 startPos;
    int i;
    public int change;

    // Start is called before the first frame update
    void Start()
    {
        light = transform.Find("light").gameObject;
        startPos = light.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        i++;
        if (i % change == 0)
        {
            i = 0;
            light.transform.position = startPos + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        }
    }
}
