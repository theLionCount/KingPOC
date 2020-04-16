using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PerlinTestSaver : MonoBehaviour
{
    Texture2D tex;
    public bool sqrd;
    public float threshold;
    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(1000, 1000);
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) > 0.7 ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes07.png", tex.EncodeToPNG());
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > 0.3f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes0303.png", tex.EncodeToPNG());
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > 0.25f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes025025.png", tex.EncodeToPNG());
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > 0.2f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes0202.png", tex.EncodeToPNG());
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > 0.35f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes035035.png", tex.EncodeToPNG());
        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > 0.4f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes0404.png", tex.EncodeToPNG());

        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, (Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) * Mathf.PerlinNoise(i / 1000f, j / 1000f + 20) * Mathf.PerlinNoise(i / 1000f, j / 1000f + 40) > 0.2f) ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes0404x05.png", tex.EncodeToPNG());

        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 25f, j / 25f) * Mathf.PerlinNoise(i / 25f + 40, j / 25f) * Mathf.PerlinNoise(i / 25f + 40, j / 25f) * Mathf.PerlinNoise(i / 250f, j / 250f + 20) * Mathf.PerlinNoise(i / 250f, j / 250f + 40) > 0.25f*0.6f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes025025025.png", tex.EncodeToPNG());

        //for (int i = 0; i < 1000; i++)
        //{
        //    for (int j = 0; j < 1000; j++)
        //    {
        //        tex.SetPixel(i, j, Mathf.PerlinNoise(i / 25f, j / 25f) + Mathf.PerlinNoise(i / 25f + 40, j / 25f) + Mathf.PerlinNoise(i / 25f + 40, j / 25f) + Mathf.PerlinNoise(i / 250f, j / 250f + 20)*2 + Mathf.PerlinNoise(i / 250f, j / 250f + 40)*2 > 4.5f ? Color.blue : Color.green);
        //    }
        //}
        //File.WriteAllBytes("lakes025025025lin.png", tex.EncodeToPNG());

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                tex.SetPixel(i, j, Color.green);
            }
        }

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                if (Mathf.PerlinNoise(i / 250f, j / 250f + 40) /*+ Mathf.PerlinNoise(i / 200f + 40, j / 200f + 40)*/ > 0.637f) tex.SetPixel(i, j, new Color(0.1f, 0.5f, 0.1f, 1));
            }
        }

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                if (Mathf.PerlinNoise(i / 100f, j / 100f) + Mathf.PerlinNoise(i / 100f + 40, j / 100f) > 1.4) tex.SetPixel(i,j,Color.blue);
            }
        }
        File.WriteAllBytes("lakes_def.png", tex.EncodeToPNG());
    }

    // Update is called once per frame
    void Update()
    {
        //if (!sqrd)
        //{
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        for (int j = 0; j < 1000; j++)
        //        {
        //            tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) > threshold ? Color.blue : Color.green);
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        for (int j = 0; j < 1000; j++)
        //        {
        //            tex.SetPixel(i, j, Mathf.PerlinNoise(i / 50.0f, j / 50.0f) * Mathf.PerlinNoise(i / 50.0f + 20, j / 50.0f) > threshold ? Color.blue : Color.green);
        //        }
        //    }
        //}
        
    }

    private void OnGUI()
    {
        //GUI.DrawTexture(new Rect(0, 0, 600, 600), tex);
    }
}
