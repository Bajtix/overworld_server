using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Resource
{
    public ItemStack itemdrop;
    public override void Hit(Item heldItem, GameItem instance)
    {
        base.Hit(heldItem, instance);
        instance.itemOwner.inventorySystem.AddItem(itemdrop.item, Mathf.RoundToInt(itemdrop.count * heldItem.damage));
        if (hp <= 0)
        {
            EntitySpawner.instance.KillEntity(GetComponent<Entity>().id);
        }
    }
}
