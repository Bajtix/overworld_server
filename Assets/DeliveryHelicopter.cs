using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryHelicopter : MonoBehaviour
{
    public Vector3 deliverTo;
    public Transform helper;
    public SpringJoint container;
    bool way = true;
    private void Update()
    {
        if(deliverTo != null)
        {
            
            if (way)
            {
                helper.LookAt(deliverTo);
                transform.rotation = Quaternion.Lerp(transform.rotation, helper.rotation, Time.deltaTime * 0.4f);
                transform.position += transform.forward * 0.5f;
            }
            else
            {
                transform.position += -transform.up * 0.2f;
                if (transform.position.y < deliverTo.y - 170)
                {
                    container.GetComponent<Entity>().additionalData = "";
                    Destroy(container);
                    way = true;
                }
            }

            if (Vector3.Distance(transform.position, deliverTo) < 10)
            {
                deliverTo = new Vector3(-1000, deliverTo.y + 100, -1000);
                way = false;
            }
        }
    }
}
