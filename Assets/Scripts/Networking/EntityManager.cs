using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    public int id;
    public string modelId;
    public int parentId;
    public Entity entity;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">New entity's id</param>
    /// <param name="modelId">Entity instance model</param>
    /// <param name="parentId">Entity's parent</param>
    public EntityManager(int id,string modelId,int parentId)
    {
        this.id = id;
        this.modelId = modelId;
        this.parentId = parentId;
    }
    /// <summary>
    /// Spawns new entity
    /// </summary>
    /// <param name="position">New entity's position</param>
    /// <param name="rotation">New entity's rotation</param>
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        entity = NetworkManager.instance.SpawnNewEntity(position,rotation, modelId,parentId);
        entity.Initialize(id,modelId,parentId);
        
        ServerSend.SpawnEntity(entity);
    }
    /// <summary>
    /// Destroys the entity
    /// </summary>
    public void Destroy()
    {
        ServerSend.KillEntity(id);
        GameObject.Destroy(entity.gameObject);
    }
}
