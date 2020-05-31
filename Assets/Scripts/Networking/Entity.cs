using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int id;
    public int modelId;
    public string additionalData;
    public bool updatePos = false;

    public void Initialize(int id,int modelId)
    {
        this.id = id;
        this.modelId = modelId;
        updatePos = true;
    }

    private void FixedUpdate()
    {
        if(updatePos)
        {
            ServerSend.EntityPosition(this);
        }
    }
}
