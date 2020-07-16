using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public ItemStack[] stacks;
    public PlayerToolbar toolbar;

    public int inventorySize = 20;
    private void Start()
    {
        stacks = new ItemStack[inventorySize];
        for (int i = 0; i < stacks.Length; i++)
        {
            stacks[i] = null;
        }
    }

    private void Update()
    {
        for (int i = 0; i < toolbar.toolBarSize; i++)
        {
            if (stacks[i] != null)
                toolbar.inventory[i] = stacks[i].item;
            else
                toolbar.inventory[i] = null;
        }
    }

    public void AddItem(Item item)
    {
        Debug.Log("Adding item " + item.name);
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null && stacks[i].count != 0)
            {
                if (stacks[i].item == item)
                {
                    if (stacks[i].count < stacks[i].item.maxStackCount)
                    {
                        stacks[i].count++;
                        return;
                    }
                }
            }
            else
            {
                stacks[i] = new ItemStack(item);
                return;
            }
        }
    }

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < count; i++)
            AddItem(item);
    }

    public void RemoveItem(Item item, int count)
    {
        for (int i = 0; i < count; i++)
            RemoveItem(item);
    }

    public bool HasItem(Item item, int count)
    {
        return CountItem(item) >= count;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                if (stacks[i].item == item)
                {
                    if (stacks[i].count > 0)
                    {
                        stacks[i].count--;
                        if (stacks[i].count <= 0)
                        {
                            stacks[i] = null;
                        }

                        return true;
                    }
                }
            }

        }
        return false;
    }

    public int CountItem(Item item)
    {
        int count = 0;
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                if (stacks[i].item == item)
                    count += stacks[i].count;
            }
        }

        return count;
    }
    public bool HasItem(Item item)
    {
        return CountItem(item) > 0;
    }

    public void Transfer(int from, int to)
    {
        if (stacks[to] == null)
        {
            stacks[to] = stacks[from];
            stacks[from] = null;
        }
        else
        {
            ItemStack s = stacks[to];
            stacks[to] = stacks[from];
            stacks[from] = s;
        }
    }
}
