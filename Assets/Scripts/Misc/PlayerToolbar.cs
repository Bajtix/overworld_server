using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolbar : MonoBehaviour
{
    public int toolBarSize = 4;
    public Item[] inventory;
    public int selected;
    public Player owner;
    public GameItem itemInstance;


    private void Start()
    {
        CycleSelection(0);
        inventory = new Item[toolBarSize];
    }
    public void CycleSelection(int slots)
    {



        selected += slots;
        //Debug.Log($"Adding {slots}, {selected}");
        if (selected >= inventory.Length)
            selected = 0;
        if (selected < 0)
            selected = inventory.Length - 1;
        //Debug.Log($"Adding {slots}, {selected}");
        if (itemInstance != null)
        {
            Debug.Log("Item instance destroyer");
            Destroy(itemInstance.gameObject);
        }

        if (inventory[selected] != null)
        {
            itemInstance = Instantiate(inventory[selected].model, transform).GetComponent<GameItem>();
            //Debug.Log("Instantiated new item");
            itemInstance.itemOwner = owner;
            //Debug.Log("Set item owner. " + itemInstance.itemOwner);
            Debug.Log("Cycling items " + itemInstance.name);
            ServerSend.PlayerInfo(owner.id, inventory[selected].name);
        }
        else
            ServerSend.PlayerInfo(owner.id, "none");
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
