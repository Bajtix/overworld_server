using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float hp = 10;
    public float stability = 0;

    public List<BuildSlot> placeSlots;
    public BuildSlot[] defaultPlaceSlots;

    private void Start()
    {
        defaultPlaceSlots = new BuildSlot[placeSlots.Count];
        placeSlots.CopyTo(defaultPlaceSlots);
    }

    private void Update()
    {
        //hp -= (1-stability) * Time.deltaTime;

        if(hp<0)
        {
            EntitySpawner.instance.KillEntity(GetComponent<Entity>().id); //decaying
        }
    }
}
