﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : Resource
{
    public ItemStack itemdrop;
    public override void Hit(Item heldItem, GameItem instance)
    {
        base.Hit(heldItem, instance);
        instance.itemOwner.inventorySystem.AddItem(itemdrop.item, Mathf.RoundToInt(itemdrop.count * heldItem.damage));
        if (hp <= 0)
        {
            EntitySpawner.instance.SpawnEntity("prp_log", transform.position + new Vector3(0, 0, 0), transform.rotation);
            EntitySpawner.instance.SpawnEntity("prp_log", transform.position + new Vector3(0, 2, 0), transform.rotation);
            EntitySpawner.instance.SpawnEntity("prp_log", transform.position + new Vector3(0, 4, 0), transform.rotation);
            EntitySpawner.instance.KillEntity(GetComponent<Entity>().id);
        }
    }
}
