using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    public string itemTag;
    public string fallingTreeEntity;

    public override void Hit(Item heldItem)
    {
        if(heldItem.itemTag == itemTag)
        {
            base.Hit(heldItem);
            base.Hit(heldItem);
        }
        else
            base.Hit(heldItem);

        if(hp <= 0)
        {
            GetComponent<ChunkObject>().chunk.RemoveFeature(GetComponent<ChunkObject>().myId);
        }
    }
}
