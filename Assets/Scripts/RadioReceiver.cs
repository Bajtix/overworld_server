using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioReceiver : Grabbable
{
    //private int channel = 0;
    private Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
        GameTimeManager.instance.SendTime();
    }

    public override void Interact(Player player = null, KeyCode button = KeyCode.E)
    {
        base.Interact(player,button);

        if (button == KeyCode.F)
        {
            Debug.Log("Radio clicked");
            EntitySpawner.instance.KillEntity(entity.id);
        }
    }
}
