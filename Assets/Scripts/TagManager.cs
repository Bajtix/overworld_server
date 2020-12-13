using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static TagManager manager;

    private void Awake()
    {
        manager = this;
    }

    public Tag stand_platform;
}
