using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    


    public BuildSlot.PlaceSlotType buildingType = BuildSlot.PlaceSlotType.Foundation;
    [System.Serializable]
    public class BuildingPart
    {
        public string part;
        public BuildSlot.PlaceSlotType type;
        public ItemStack cost;
    }

    //public string[] parts;
    //public BuildSlot.PlaceSlotType[] types = { BuildSlot.PlaceSlotType.Foundation, BuildSlot.PlaceSlotType.Wall , BuildSlot.PlaceSlotType.Wall, BuildSlot.PlaceSlotType.Floor};

    public BuildingPart[] buildingParts;
    public int selectedPart = 0;
    public Transform look;
    public Quaternion rot = Quaternion.identity;
    public string previewID;
    private Player player;
    public GameObject preview;

    private void Start()
    {
        int id = EntitySpawner.instance.SpawnEntity(previewID, transform.position, transform.rotation);
        preview = Server.entities[id].entity.gameObject;
        player = GetComponent<Player>();
    }

    public void UpdatePreview()
    {
        

        buildingType = buildingParts[selectedPart].type;
        preview.GetComponent<Entity>().additionalData = buildingParts[selectedPart].part.ToString();
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 10f, MaskBuilds(buildingType)))
        {
            List<int> layers = new List<int>();
            layers.AddRange(new int[] { LayerMask.NameToLayer("BuildSlot Foundation"), LayerMask.NameToLayer("BuildSlot Wall"), LayerMask.NameToLayer("BuildSlot Floor"), LayerMask.NameToLayer("BuildSlot Inner") });
            if (layers.Contains(hit.collider.gameObject.layer))
            {
                preview.transform.position = hit.collider.transform.position;
                if (buildingType == BuildSlot.PlaceSlotType.Main)
                    preview.transform.rotation = hit.collider.transform.rotation * rot;
                else
                    preview.transform.rotation = hit.collider.transform.rotation;
            }
            else
            {
                preview.transform.position = Vector3.zero;
            }
            
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
                    return LayerMask.GetMask("BuildSlot Foundation", "Default", "Building");
                case BuildSlot.PlaceSlotType.Wall:
                    return LayerMask.GetMask("BuildSlot Wall", "Default", "Building");
                case BuildSlot.PlaceSlotType.Floor:
                    return LayerMask.GetMask("BuildSlot Floor", "Default", "Building");
                case BuildSlot.PlaceSlotType.Main:
                    return LayerMask.GetMask("BuildSlot Inner", "Default", "Building");
                default:
                    return LayerMask.GetMask("Default", "Building");
            }
        }
        else
        {
            switch (t)
            {
                case BuildSlot.PlaceSlotType.Foundation:
                    return LayerMask.GetMask("BuildSlot Foundation", "Building");
                case BuildSlot.PlaceSlotType.Wall:
                    return LayerMask.GetMask("BuildSlot Wall", "Building");
                case BuildSlot.PlaceSlotType.Floor:
                    return LayerMask.GetMask("BuildSlot Floor", "Building");
                case BuildSlot.PlaceSlotType.Main:
                    return LayerMask.GetMask("BuildSlot Inner", "Building");
                default:
                    return LayerMask.GetMask("Default", "Building");
            }
        }
    }

    public void BuildButton()
    {
        
        if (player.inventorySystem.HasItem(buildingParts[selectedPart].cost.item, buildingParts[selectedPart].cost.count))
        {
            player.inventorySystem.RemoveItem(buildingParts[selectedPart].cost.item, buildingParts[selectedPart].cost.count);
            RaycastHit hit;
            if (Physics.Raycast(look.position, look.forward, out hit, 10f, MaskBuilds(buildingType)))
            {
                if (hit.collider != null && hit.collider.gameObject.tag != "Building")
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
        else
        {
            ServerSend.SendInfo(player.id, "Can't afford. Building Cost: " + buildingParts[selectedPart].cost.count + " x " + buildingParts[selectedPart].cost.item.name);
        }
    }

    public void DestroyButton()
    {
        RaycastHit hit;
        if (Physics.Raycast(look.position, look.forward, out hit, 10f, LayerMask.GetMask("Building")))
        {
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Building>() != null)
                {
                    EntitySpawner.instance.KillEntity(hit.collider.GetComponent<Entity>().id);
                }
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
                    EntitySpawner.instance.SpawnEntity(buildingParts[selectedPart].part, collider.transform.position, collider.transform.rotation * rot) 
                    :
                    EntitySpawner.instance.SpawnEntity(buildingParts[selectedPart].part, collider.transform.position, collider.transform.rotation); //spawns the selected building type in the selected slot and gets id.

                //Collider[] hitSlots = Physics.OverlapSphere(collider.transform.position, 0.1f, MaskBuilds(buildingType));

                BuildSlot newSlot = Server.entities[id].entity.gameObject.AddComponent<BuildSlot>();
                newSlot.type = BuildSlot.PlaceSlotType.Occupied; //configure "me" as a slot

                //List<Building> buildings = new List<Building>();
                /*foreach(Collider col in hitSlots) //list of buildings whom slots were hit
                {
                    Building b = col.transform.parent.GetComponent<Building>();
                    buildings.Add(b);
                    col.gameObject.SetActive(false);
                    b.placeSlots[b.placeSlots.IndexOf(slot)] = newSlot;
                }*/
                //Building b = collider.transform.parent.GetComponent<Building>();

                Building b = collider.transform.parent.GetComponent<Building>();
                b.placeSlots[b.placeSlots.IndexOf(slot)] = newSlot;
                

            }
        }
        else if (collider.GetComponent<TerrainGenerator>() != null)
        {
            if(buildingType == BuildSlot.PlaceSlotType.Foundation)
                EntitySpawner.instance.SpawnEntity(buildingParts[0].part, hit.point, Quaternion.LookRotation(Vector3.up));
        }


    }
}
