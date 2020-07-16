using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    public string itemTag;
    public string fallingTreeEntity;

    public override void Hit(Item heldItem, GameItem instance)
    {
        if(heldItem.itemTag == itemTag)
        {
            base.Hit(heldItem, instance);
            base.Hit(heldItem, instance);
        }
        else
            base.Hit(heldItem, instance);

        if(hp <= 0)
        {
            instance.itemOwner.inventorySystem.AddItem(drop, Random.Range(20, 50));
            GetComponent<ChunkObject>().chunk.RemoveFeature(GetComponent<ChunkObject>().myId);
        }
    }
}
