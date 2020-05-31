using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnitySpawner : MonoBehaviour
{
    public bool clickToSpawnNew = false;
    public static EnitySpawner instance;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (clickToSpawnNew)
        {
            Debug.Log("Spawning new entity");
            int id = Server.entities.Values.Count;
            Server.entities.Add(id,new EntityManager(id,1));
            RaycastHit hit;
            Physics.Raycast(new Vector3(800, 300, 800),Vector3.down,out hit);
            Server.entities[id].Spawn(hit.point + Vector3.up*10);
            clickToSpawnNew = false;
        }
    }

    public void Spawn(Vector3 v)
    {
        Debug.Log("Spawning new entity");
        int id = Server.entities.Values.Count;
        Server.entities.Add(id, new EntityManager(id, 1));
        RaycastHit hit;
        Physics.Raycast(v, Vector3.down, out hit);
        Server.entities[id].Spawn(hit.point + Vector3.up * 10);
    }
}
