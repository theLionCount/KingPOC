using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    public GameObject triggerObj;
    AreaTrigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = triggerObj.GetComponent<AreaTrigger>();
        trigger.areaClear += Trigger_areaClear;
        trigger.playerEntered += Trigger_playerEntered;
        transform.localScale = Vector3.zero;
    }

    private void Trigger_playerEntered(object sender, System.EventArgs e)
    {
        transform.localScale = Vector3.one;
    }

    private void Trigger_areaClear(object sender, System.EventArgs e)
    {
        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
