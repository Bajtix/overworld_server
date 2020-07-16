using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterInventory : MonoBehaviour
{
    public Item[] items;
    public InventorySystem inventory;
    private void Start()
    {
        foreach (Item i in items)
            inventory.AddItem(i);
        //inventory.AddItem(items[0]);
    }
}
