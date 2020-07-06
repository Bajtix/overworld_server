using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stack : MonoBehaviour
{
    public Item item;
    public int count;

    public Stack(Item item, int v)
    {
        this.item = item;
        this.count = v;
    }
}
