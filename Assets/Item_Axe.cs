using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Axe : GameItem
{
    private Transform look;

    private void Start()
    {
        look = itemOwner.look;
    }

    public override void MainClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 4f))
        {
            if (hit.collider != null)
                if (hit.collider.GetComponent<ColliderPart>() != null)
                {
                    ChunkObject i = hit.collider.GetComponent<ColliderPart>().obj.GetComponent<ChunkObject>();
                    i.chunk.RemoveFeature(i.myId);
                }
        }
    }

    
}
