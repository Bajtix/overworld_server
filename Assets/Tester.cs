using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public TerrainGenerator chunk;
    public GameObject cube;
    public bool clickHereToRemoveObject;
    public bool clickHereToAddObject;

    private void Update()
    {
        
        if (clickHereToRemoveObject)
            chunk.RemoveFeature(0,true);
        if (clickHereToAddObject)
        {
            GameObject go = Instantiate(cube, chunk.transform.position, Quaternion.identity);
            chunk.AddFeature(go, true);
        }

        clickHereToRemoveObject = false;
        clickHereToAddObject = false;
    }
}
