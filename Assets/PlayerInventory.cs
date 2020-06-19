using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> inventory;
    public int selected;
    public Player owner;
    public GameItem itemInstance;


    private void Start()
    {
        CycleSelection(0);
    }
    public void CycleSelection(int slots)
    {



        selected += slots;
        Debug.Log($"Adding {slots}, {selected}");
        if (selected >= inventory.Count)
            selected = 0;
        if (selected < 0)
            selected = inventory.Count - 1;
        Debug.Log($"Adding {slots}, {selected}");
        if (itemInstance != null)
        {
            Debug.Log("Item instance destroyer");
            Destroy(itemInstance.gameObject);
        }

        
        itemInstance = Instantiate(inventory[selected].model, transform).GetComponent<GameItem>();
        //Debug.Log("Instantiated new item");
        itemInstance.itemOwner = owner;
        //Debug.Log("Set item owner. " + itemInstance.itemOwner);
        Debug.Log("Cycling items " + itemInstance.name);

        ServerSend.PlayerInfo(owner.id, inventory[selected].name);
    }

    private void Update()
    {
        if (itemInstance != null)
            itemInstance.Tick();
    }

    public void LeftClick()
    {
        if (itemInstance != null)
            itemInstance.MainClick();
    }

    public void RightClick()
    {
        if (itemInstance != null)
            itemInstance.SecondaryClick();
    }

    public void Reload()
    {
        if (itemInstance != null)
            itemInstance.Reload();
    }

    public void Alternative()
    {
        if (itemInstance != null)
            itemInstance.Alternative();
    }
}
