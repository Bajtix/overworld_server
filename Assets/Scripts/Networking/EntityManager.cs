using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    public int id;
    public int modelId;
    public Entity entity;

    public EntityManager(int id,int modelId)
    {
        this.id = id;
        this.modelId = modelId;
    }

    public void Spawn(Vector3 position)
    {
        entity = NetworkManager.instance.SpawnNewEntity(position,modelId);
        entity.Initialize(id,modelId);
        
        ServerSend.SpawnEntity(entity);
    }
}
