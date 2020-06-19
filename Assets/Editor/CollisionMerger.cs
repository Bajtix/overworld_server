using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CollisionMerger : EditorWindow
{
    [MenuItem("Tools/Collision Merger")]
    public static void ShowWindow()
    {
        GetWindow<CollisionMerger>("Collision Tool");
    }
    GameObject target;
    private void OnGUI()
    {
        target = (GameObject)EditorGUILayout.ObjectField(target, typeof(GameObject),true);
        if(GUILayout.Button("Merge"))
        {
            GameObject[] selected = Selection.gameObjects;
            foreach(GameObject g in selected)
            {
                CapsuleCollider oc = g.GetComponent<CapsuleCollider>();
                CapsuleCollider cc = target.AddComponent<CapsuleCollider>();
                cc.direction = oc.direction;
                cc.radius = oc.radius;
                cc.height = oc.height;
                cc.center = oc.center;
            }
        }
    }

    
}
