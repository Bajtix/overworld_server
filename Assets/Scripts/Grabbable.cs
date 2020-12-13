using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{

    public Player boundTo;
    [SerializeField]
    private Rigidbody rb;

    private Quaternion irot;
    private void Start()
    {
        if(rb == null)
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Player player = null, KeyCode button = KeyCode.E)
    {
        if (button != KeyCode.E) return;

        if (player != null && player != boundTo)
        {
            if (boundTo == null)
            {
                boundTo = player;
                irot = transform.rotation * Quaternion.Euler(0,90,0);
            }
            else
                boundTo = null;
        }
        
    }

    private void FixedUpdate()
    {
        if(boundTo != null)
        {
            boundTo.holding = this;
            
            Vector3 dest = boundTo.look.position + boundTo.look.forward * 2f;
            Vector3 dvec = dest - transform.position;
            float dist = Mathf.Clamp(Vector3.Distance(dest, transform.position)*1.1f,0f,5f);
            float force = 26f;

            rb.AddForce(dvec * dist * force);
            rb.MoveRotation(boundTo.look.rotation * irot);
            rb.drag = 2;
            rb.angularDrag = 2;
        }
        else
            rb.angularDrag = 0;
    }
}
