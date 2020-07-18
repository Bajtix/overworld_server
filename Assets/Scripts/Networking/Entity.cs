using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int id;
    public string modelId;
    public int parentId = -1;
    public string additionalData;
    public bool updatePos = true;

    /// <summary>
    /// Initializes entity with data
    /// </summary>
    /// <param name="id">The entities id, should be the same as server's</param>
    /// <param name="modelId">The spawned entity. </param>
    /// <param name="parentId">Parent entity</param>
    public void Initialize(int id,string modelId, int parentId)
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
