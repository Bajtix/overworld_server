using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureFrame : Interactable
{
    private Entity bound;

    public float offset = .05f;

    private void OnCollisionEnter(Collision collision)
    {
        if (bound != null) return;
        if (collision.collider.gameObject.name.Contains("prp_pic"))
        {
            Entity ce = collision.collider.gameObject.GetComponent<Entity>();
            bound = ce;
            bound.transform.position = transform.position + transform.forward * offset;
            bound.transform.rotation = transform.rotation * Quaternion.Euler(90,0,0);

            ce.GetComponent<Grabbable>().boundTo.holding = null;
            ce.GetComponent<Grabbable>().boundTo = null;
            ce.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (bound == null) return;

        bound.transform.position = transform.position + transform.forward * offset;
        bound.transform.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);

        Debug.Log($"picture frame test debug: my rot: {transform.rotation}  server my rot {Server.entities[GetComponent<Entity>().id].entity.transform.rotation}");
    }


    public override void Interact(Player player = null, KeyCode button = KeyCode.E)
    {
        if (button == KeyCode.E)
        {
            if (bound != null)
            {
                bound.transform.position += transform.forward * 2f;
                bound.GetComponent<Rigidbody>().isKinematic = false;
                bound = null;
            }
        }
        if (button == KeyCode.F)
        {
            if (bound != null)
            {
                bound.GetComponent<Rigidbody>().isKinematic = false;
                bound.transform.position += transform.forward * 2f;
                bound = null;
            }
            EntitySpawner.instance.KillEntity(GetComponent<Entity>().id);
        }
    }
}
