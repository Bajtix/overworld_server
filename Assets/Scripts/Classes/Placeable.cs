using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : GameItem
{
    public string entityToSpawn;
    private Transform look;
    public bool lookAtPlacement;

    private LayerMask dmask;


    public bool faceHitNormalOnly;
    public bool useTag;
    public LayerMask layerPlaceOn;
    public Tag[] tagOn;

    private void Start()
    {
        look = itemOwner.look;
        dmask = _GameUtilityToolset.GetPhysicsLayerMask(0);
    }
    public override void MainClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 14f,dmask))
        {

            if (layerPlaceOn == (layerPlaceOn | (1 << hit.collider.gameObject.layer)) && (!useTag || hit.collider.gameObject.HasAnyTag(tagOn)) )
            {
                Entity e = EntitySpawner.instance.SpawnEntityReturn(entityToSpawn, hit.point, Quaternion.identity);
                if (!faceHitNormalOnly)
                {
                    e.transform.LookAt(look.position);
                    e.transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal) * Quaternion.Euler(0, e.transform.rotation.eulerAngles.y - 90, 0);
                    e.SendEntityData();
                }
                else
                {
                    e.transform.rotation = Quaternion.LookRotation(hit.normal);
                    e.SendEntityData();
                }
            }
        }
    }
}
