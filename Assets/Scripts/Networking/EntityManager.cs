using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    public int id;
    public int modelId;
    public int parentId;
    public Entity entity;

    public EntityManager(int id,int modelId,int parentId)
    {
        this.id = id;
        this.modelId = modelId;
        this.parentId = parentId;
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        entity = NetworkManager.instance.SpawnNewEntity(position,rotation, modelId,parentId);
        entity.Initialize(id,modelId,parentId);
        
        ServerSend.SpawnEntity(entity);
    }

    public void Destroy()
    {
        ServerSend.KillEntity(id);
        GameObject.Destroy(entity.gameObject);
    }
}
