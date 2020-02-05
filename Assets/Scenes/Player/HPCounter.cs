using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HPCounter : MonoBehaviour
{
    public Swordsman body;
    public TextElement text;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInParent<Swordsman>();
        text = GetComponent<TextElement>();
    }

    // Update is called once per frame
    void Update()
    {
     //   text.text = body.health.ToString();
    }
}
