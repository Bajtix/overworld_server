﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    


    public float hp = 10;
    public float stability = 0;
    
    public List<BuildSlot> placeSlots;

    private void Update()
    {
        //hp -= (1-stability) * Time.deltaTime;

        if(hp<0)
        {
            EnitySpawner.instance.KillEntity(GetComponent<Entity>().id); //decaying
        }
    }
}