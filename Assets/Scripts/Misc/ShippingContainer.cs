﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingContainer : Interactable
{
    public string spawnProp;

    public override void Interact()
    {
        EntitySpawner.instance.SpawnEntity(spawnProp, transform.position + new Vector3(0,5,0), Quaternion.identity);
        EntitySpawner.instance.KillEntity(GetComponent<Entity>().id);
    }
}
