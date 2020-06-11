using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int id;
    public int modelId;
    public int parentId = -1;
    public string additionalData;
    public bool updatePos = true;

    public void Initialize(int id,int modelId, int parentId)
    {
        this.id = id;
        this.modelId = modelId;
        this.parentId = parentId;
    }


    private void FixedUpdate()
    {
        if(updatePos)
        {
            ServerSend.EntityPosition(this);
        }
    }
}
