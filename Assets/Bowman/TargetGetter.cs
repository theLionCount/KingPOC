using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGetter : MonoBehaviour, ITargetGettr
{
    static Vector3 half = Vector2.zero;

    // Use this for initialization
    void Start()
    {
        half = -new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        half = -new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public Vector2 getTarget()
    {
        return (Input.mousePosition + half).normalized;
    }

}
