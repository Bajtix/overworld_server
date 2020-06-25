using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnitySpawner : MonoBehaviour
{
    public static EnitySpawner instance;

    public bool spawn = false;

    public Dictionary<int, string> cars = new Dictionary<int, string>()
    {
        { 1, "vec_rusty-truck"},
        { 0, "vec_rusty-buggy"},
        { -1, "cancel"}
    };

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (spawn)
        {
            SpawnCar(new Vector3(200, 100, 100));
        }
        spawn = false;
    }


    public void SpawnCar(Vector3 v, int car = -1)
    {
        /*Debug.Log("Spawning new entity");
        int id = Server.entities.Values.Count;        
        RaycastHit hit;
        Physics.Raycast(v, Vector3.down, out hit);

        Server.entities.Add(id, new EntityManager(id, "vec_rusty-truck", -9999));
        Server.entities[id].Spawn(hit.point + Vector3.up * 10,Quaternion.identity);*/

        Debug.Log("Delivery started, code " + car);
        if (car == -1) return;
        
        Vector3 p = v + new Vector3(100, 100, 208);
        int id = SpawnNewEntity("prp_cargobob", p, Quaternion.identity);
        int cid = SpawnNewEntity("prp_container", p - new Vector3(0,0,0), Quaternion.identity);
        Server.entities[id].entity.GetComponent<DeliveryHelicopter>().deliverTo = v + new Vector3(0, 100, 0);
        
        Server.entities[cid].entity.GetComponent<SpringJoint>().connectedBody = Server.entities[id].entity.GetComponent<Rigidbody>();
        Server.entities[cid].entity.GetComponent<ShippingContainer>().spawnProp = cars[car];
        Server.entities[cid].entity.GetComponent<Entity>().additionalData = id.ToString() ;
        Server.entities[id].entity.GetComponent<DeliveryHelicopter>().container = Server.entities[cid].entity.GetComponent<SpringJoint>();

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
