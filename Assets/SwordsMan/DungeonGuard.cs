using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGuard : SwordsmanController
{
    public GameObject triggerObj;
    AreaTrigger trigger;
    bool agro;

    // Start is called before the first frame update
    protected override void Start()
    {
        trigger = triggerObj.GetComponent<AreaTrigger>();
        trigger.playerEntered += Trigger_playerEntered;
        agro = false;
        base.Start();
    }

    private void Trigger_playerEntered(object sender, System.EventArgs e)
    {
        agro = true;
    }

    protected override void FixedUpdate()
    {
        if (agro) base.FixedUpdate();
    }
}
