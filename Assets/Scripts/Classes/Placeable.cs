using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : GameItem
{
    public string entityToSpawn;
    private Transform look;
    private void Start()
    {
        look = itemOwner.look;
    }
    public override void MainClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 14f,LayerMask.GetMask("Default","Building")))
        {
            Entity e = EntitySpawner.instance.SpawnEntityReturn(entityToSpawn, hit.point, Quaternion.identity);
            e.transform.LookAt(look.position);
            e.transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal) * e.transform.rotation;
        }
    }
}
