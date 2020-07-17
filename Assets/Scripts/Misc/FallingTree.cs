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
            EnitySpawner.instance.KillEntity(GetComponent<Entity>().id);
    }
}
