using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public Stack[] stacks;

    public int inventorySize = 20;
    private void Start()
    {
        stacks = new Stack[inventorySize];
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if(stacks[i] != null)
            {
                if(stacks[i].item == item)
                {
                    if(stacks[i].count < stacks[i].item.maxStackCount)
                    {
                        stacks[i].count++;
                        return;
                    }
                }
            }
            else
            {
                stacks[i] = new Stack(item, 1);
            }
        }
    }
}
