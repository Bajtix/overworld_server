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

    public override void SecondaryClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 14f))
            EntitySpawner.instance.SpawnEntity("prp_radio_receiver_1", hit.point, Quaternion.identity);
    }

    public override void MainClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 4f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<ColliderPart>() != null)
                {
                    hit.collider.GetComponent<ColliderPart>().obj.GetComponent<Resource>().Hit(myItem, this);
                }
                else if(hit.collider.GetComponent<Resource>() != null)
                {
                    hit.collider.GetComponent<Resource>().Hit(myItem, this);
                }
            }
        }
    }

    

}
