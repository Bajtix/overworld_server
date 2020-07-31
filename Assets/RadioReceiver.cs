using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioReceiver : Interactable
{
    private int channel = 0;
    private Entity entity;

    private void Start()
    {
        entity.additionalData = channel.ToString();
        GameTimeManager.instance.SendTime();
    }

    public override void Interact()
    {
        channel++;
        entity.additionalData = channel.ToString();
        entity.SendEntityData();
    }
}
