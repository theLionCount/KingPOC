using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    public Color nightColor;
    public Color dayColor;
    Color currentColor;

    public float Length;
    public float SRstart, SREnd, SSStart, SSEnd;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        time++;
        float tt = time % Length;
        float ipd = tt / Length;
        if (ipd < SRstart || ipd > SSEnd) currentColor = nightColor;
        if (ipd > SREnd && ipd < SSStart) currentColor = dayColor;
        if (ipd > SRstart && ipd < SREnd) currentColor = Color.Lerp(nightColor, dayColor, (ipd - SRstart) / (SREnd - SRstart));
        if (ipd > SSStart && ipd < SSEnd) currentColor = Color.Lerp(dayColor, nightColor, (ipd - SSStart) / (SSEnd - SSStart));
    }

    public Color getAmbientColor => currentColor;
}
