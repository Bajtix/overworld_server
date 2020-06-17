using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnitySpawner : MonoBehaviour
{
    public static EnitySpawner instance;

    private void Awake()
    {
        instance = this;
    }


    public void SpawnCar(Vector3 v)
    {
        Debug.Log("Spawning new entity");
        int id = Server.entities.Values.Count;        
        RaycastHit hit;
        Physics.Raycast(v, Vector3.down, out hit);

        Server.entities.Add(id, new EntityManager(id, "vc_rusty-buggy", -9999));
        Server.entities[id].Spawn(hit.point + Vector3.up * 10,Quaternion.identity);

    }

    public int SpawnNewEntity(string modelId, Vector3 position, Quaternion rotation)
    {
        Debug.Log("Spawning new entity");
        int id = _GameUtilityToolset.FirstFree(Server.entities,0);

        Server.entities.Add(id, new EntityManager(id, modelId, -9999));
        Server.entities[id].Spawn(position,rotation);

        return id;
    }

    public void KillEntity(int id)
    {
        Debug.Log("Killing entity");
        Server.entities[id].Destroy();
        Server.entities.Remove(id);
    }

}
