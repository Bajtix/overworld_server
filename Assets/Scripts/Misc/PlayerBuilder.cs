using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    public BuildSlot.PlaceSlotType buildingType = BuildSlot.PlaceSlotType.Foundation;
    public int[] parts = {2,3,4,5};
    public BuildSlot.PlaceSlotType[] types = { BuildSlot.PlaceSlotType.Foundation, BuildSlot.PlaceSlotType.Wall , BuildSlot.PlaceSlotType.Wall, BuildSlot.PlaceSlotType.Floor};
    public int selectedPart = 0;
    public Transform look;
    public Quaternion rot = Quaternion.identity;
    public int previewID;

    private GameObject preview;

    private void Start()
    {
        int id = EnitySpawner.instance.SpawnNewEntity(previewID, transform.position, transform.rotation);
        preview = Server.entities[id].entity.gameObject;
    }

    private void Update()
    {
        buildingType = types[selectedPart];
        preview.GetComponent<Entity>().additionalData = parts[selectedPart].ToString();
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 10f, MaskBuilds(buildingType,false)))
        {
            preview.transform.position = hit.collider.transform.position;
            if (buildingType == BuildSlot.PlaceSlotType.Main)
                preview.transform.rotation = hit.collider.transform.rotation * rot;
            else
                preview.transform.rotation = hit.collider.transform.rotation;
        }
        else
            preview.GetComponent<Entity>().additionalData = "0";
    }

    public int MaskBuilds(BuildSlot.PlaceSlotType t,bool def = true)
    {
        if (def)
        {
            switch (t)
            {
                case BuildSlot.PlaceSlotType.Foundation:
                    return LayerMask.GetMask("BuildSlot Foundation", "Default");
                case BuildSlot.PlaceSlotType.Wall:
                    return LayerMask.GetMask("BuildSlot Wall", "Default");
                case BuildSlot.PlaceSlotType.Floor:
                    return LayerMask.GetMask("BuildSlot Floor", "Default");
                case BuildSlot.PlaceSlotType.Main:
                    return LayerMask.GetMask("BuildSlot Inner", "Default");
                default:
                    return LayerMask.GetMask("Default");
            }
        }
        else
        {
            switch (t)
            {
                case BuildSlot.PlaceSlotType.Foundation:
                    return LayerMask.GetMask("BuildSlot Foundation");
                case BuildSlot.PlaceSlotType.Wall:
                    return LayerMask.GetMask("BuildSlot Wall");
                case BuildSlot.PlaceSlotType.Floor:
                    return LayerMask.GetMask("BuildSlot Floor");
                case BuildSlot.PlaceSlotType.Main:
                    return LayerMask.GetMask("BuildSlot Inner");
                default:
                    return LayerMask.GetMask("Default");
            }
        }
    }

    public void BuildButton()
    {
        
        
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 10f, MaskBuilds(buildingType)))
        {
            if (hit.collider != null)
            {
                Build(hit.collider, hit);
            }

        }
        else
        {
            hit.point = look.position + look.forward * 10;
            Build(null, hit);
        }
    }

    public void DestroyButton(Transform look)
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 10f, LayerMask.GetMask("Building")))
        {
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Entity>() != null)
                    EnitySpawner.instance.KillEntity(hit.collider.GetComponent<Entity>().id);
            }

        }
    }

    public void Build(Collider collider, RaycastHit hit)
    {
        if (collider == null) return;

        if (collider.GetComponent<BuildSlot>() != null)
        {
            BuildSlot slot = collider.GetComponent<BuildSlot>();
            if (slot.type == buildingType)
            {
                   
                int id = buildingType == BuildSlot.PlaceSlotType.Main ? 
                    EnitySpawner.instance.SpawnNewEntity(parts[selectedPart], collider.transform.position, collider.transform.rotation * rot) 
                    :
                    EnitySpawner.instance.SpawnNewEntity(parts[selectedPart], collider.transform.position, collider.transform.rotation); //spawns the selected building type in the selected slot and gets id.

                Collider[] hitSlots = Physics.OverlapSphere(collider.transform.position, 0.1f, MaskBuilds(buildingType));

                BuildSlot newSlot = Server.entities[id].entity.gameObject.AddComponent<BuildSlot>();
                newSlot.type = BuildSlot.PlaceSlotType.Occupied; //configure "me" as a slot

                List<Building> buildings = new List<Building>();
                foreach(Collider col in hitSlots) //list of buildings whom slots were hit
                {
                    Building b = col.transform.parent.GetComponent<Building>();
                    buildings.Add(b);
                    col.gameObject.SetActive(false);
                    b.placeSlots[b.placeSlots.IndexOf(slot)] = newSlot;
                }
                //Building b = collider.transform.parent.GetComponent<Building>();
                
               

                
                
            }
        }
        else if (collider.GetComponent<TerrainGenerator>() != null)
        {
            if(buildingType == BuildSlot.PlaceSlotType.Foundation)
            EnitySpawner.instance.SpawnNewEntity(2, hit.point, Quaternion.LookRotation(Vector3.up));
        }


    }
}
