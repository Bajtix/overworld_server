using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntitySpawner : MonoBehaviour
{
    public static EntitySpawner instance;
    private void Awake()
    {
        instance = this;
    }

    public Dictionary<int, string> cars = new Dictionary<int, string>()
    {
        { 1, "vec_rusty-truck"},
        { 0, "vec_rusty-buggy"},
        { -1, "cancel"}
    };


    /// <summary>
    /// Starts the Car Delivery
    /// </summary>
    /// <param name="v"></param>
    /// <param name="car"></param>
    public void SpawnCar(Vector3 v, int car = -1)
    {
        Debug.Log("Delivery started, code " + car);
        if (car == -1)
        {
            return;
        }

        Vector3 p = v + new Vector3(100, 100, 208);
        int id = SpawnEntity("prp_cargobob", p, Quaternion.identity);
        int cid = SpawnEntity("prp_container", p - new Vector3(0, 0, 0), Quaternion.identity);
        Server.entities[id].entity.GetComponent<DeliveryHelicopter>().deliverTo = v + new Vector3(0, 100, 0);

        Server.entities[cid].entity.GetComponent<SpringJoint>().connectedBody = Server.entities[id].entity.GetComponent<Rigidbody>();
        Server.entities[cid].entity.GetComponent<ShippingContainer>().spawnProp = cars[car];
        Server.entities[cid].entity.GetComponent<Entity>().additionalData = id.ToString();
        Server.entities[id].entity.GetComponent<DeliveryHelicopter>().container = Server.entities[cid].entity.GetComponent<SpringJoint>();

    }
    /// <summary>
    /// Spawns New Entity
    /// </summary>
    /// <param name="modelId">Entity Prefab ID</param>
    /// <param name="position">Entity Position</param>
    /// <param name="rotation">Entity Rotation</param>
    /// <returns>The id of the new entity</returns>
    public int SpawnEntity(string modelId, Vector3 position, Quaternion rotation, object data = null)
    {
        int id = _GameUtilityToolset.FirstFree(Server.entities, 0);

        Server.entities.Add(id, new EntityManager(id, modelId, -9999));
        Server.entities[id].Spawn(position, rotation,data);

        return id;
    }
    /// <summary>
    /// Spawns New Entity
    /// </summary>
    /// <param name="modelId">Entity Prefab ID</param>
    /// <param name="position">Entity Position</param>
    /// <param name="rotation">Entity Rotation</param>
    /// <returns>The new entity</returns>
    public Entity SpawnEntityReturn(string modelId, Vector3 position, Quaternion rotation)
    {
        return Server.entities[SpawnEntity(modelId, position, rotation)].entity;
    }

    /// <summary>
    /// Kills the entity
    /// </summary>
    /// <param name="id">Entity to kill</param>
    public void KillEntity(int id)
    {
        Server.entities[id].Destroy();
        Server.entities.Remove(id);
    }
    /// <summary>
    /// Kills the entity
    /// </summary>
    /// <param name="entity">Entity to kill</param>
    public void KillEntity(Entity entity)
    {
        int id = entity.id;
        Server.entities[id].Destroy();
        Server.entities.Remove(id);
    }



}
#if UNITY_EDITOR
[CustomEditor(typeof(EntitySpawner))]
class EntitySpawnerEditor : Editor
{
    private string eName = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        eName = EditorGUILayout.TextField("Entity to spawn",eName);
        if(GUILayout.Button("Spawn"))
        {
            ((EntitySpawner)target).SpawnEntity(eName, Vector3.zero, Quaternion.identity);
        }

        GUILayout.EndHorizontal();
    }
}
#endif
