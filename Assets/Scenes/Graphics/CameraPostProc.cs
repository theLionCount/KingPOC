using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProc : MonoBehaviour
{
    public Material TimeOfDaymaterial;
    TimeOfDay tod;

    // Start is called before the first frame update
    void Start()
    {
        tod = GameObject.Find("Ambience").GetComponent<TimeOfDay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        TimeOfDaymaterial.SetColor("_Color", tod.getAmbientColor);
        Graphics.Blit(source, destination, TimeOfDaymaterial);
    }
}
