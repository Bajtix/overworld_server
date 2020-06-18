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
        if (selected >= inventory.Count)
            selected = 0;
        if (selected < 0)
            selected = inventory.Count - 1;

        if(itemInstance != null)
            Destroy(itemInstance);

        Debug.Log("Cycling items");
        itemInstance = Instantiate(inventory[selected].model, transform).GetComponent<GameItem>();
        Debug.Log("Instantiated new item");
        itemInstance.itemOwner = owner;
        Debug.Log("Set item owner. " + itemInstance.itemOwner);
    }

    private void Update()
    {
        if(itemInstance!=null)
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

}
