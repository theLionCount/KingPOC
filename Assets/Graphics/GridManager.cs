using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int w, h;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GridNW").transform.position = new Vector3(-w, h, 0);
        GameObject.Find("GridN").transform.position = new Vector3(0, h, 0);
        GameObject.Find("GridNE").transform.position = new Vector3(w, h, 0);
        GameObject.Find("GridW").transform.position = new Vector3(-w, 0, 0);
        GameObject.Find("GridC").transform.position = new Vector3(0, 0, 0);
        GameObject.Find("GridE").transform.position = new Vector3(w, 0, 0);
        GameObject.Find("GridSW").transform.position = new Vector3(-w, -h, 0);
        GameObject.Find("GridS").transform.position = new Vector3(0, -h, 0);
        GameObject.Find("GridSE").transform.position = new Vector3(w, -h, 0);

        GameObject.Find("GridNW").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridNW").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridN").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridN").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridNE").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridNE").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridW").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridW").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridC").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridC").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridE").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridE").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridSW").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridSW").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridS").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridS").GetComponent<OutsideCreator>().h = h;

        GameObject.Find("GridSE").GetComponent<OutsideCreator>().w = w;
        GameObject.Find("GridSE").GetComponent<OutsideCreator>().h = h;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
