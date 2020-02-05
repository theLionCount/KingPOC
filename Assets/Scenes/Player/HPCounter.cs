using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HPCounter : MonoBehaviour
{
    public Swordsman body;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInParent<Swordsman>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(1, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.9f, 0.9f, 0.1f, 1.0f);
        string text = string.Format("{0} / 6", body.health);
        GUI.Label(rect, text, style);
    }
}
