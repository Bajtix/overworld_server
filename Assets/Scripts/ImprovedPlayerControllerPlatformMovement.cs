using UnityEngine;

public class ImprovedPlayerControllerPlatformMovement : MonoBehaviour
{
    public Transform activePlatform;
    private CharacterController controller;

    [SerializeField]
    private Rigidbody rcontroller;

    private Vector3 moveDirection;
    private Vector3 activeGlobalPlatformPoint;
    private Vector3 activeLocalPlatformPoint;
    private Quaternion activeGlobalPlatformRotation;
    private Quaternion activeLocalPlatformRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if(rcontroller == null && controller == null)
            rcontroller = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (activePlatform != null)
        {
            Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
            moveDirection = newGlobalPlatformPoint - activeGlobalPlatformPoint;

            if (moveDirection.magnitude > 0f)
            {
                if (controller != null)
                    controller.Move(moveDirection);
                else
                    rcontroller.MovePosition(transform.position + moveDirection);

                
            }
            if (activePlatform)
            {
                // Support moving platform rotation
                Quaternion newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;
                Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);

                // Prevent rotation of the local up vector
                rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;
                transform.rotation = rotationDiff * transform.rotation;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                UpdateMovingPlatform();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Make sure we are really standing on a straight platform *NEW*
        // Not on the underside of one and not falling down from it either!
        if (hit.moveDirection.y < -0.9 && Vector3.Angle(hit.normal, Vector3.up) < controller.slopeLimit && hit.collider.HasTag(TagManager.manager.stand_platform) && controller.GetComponent<Player>().seatIn == null)
        {
            if (activePlatform != hit.collider.transform)
            {
                activePlatform = hit.collider.transform;
                UpdateMovingPlatform();
            }
        }
        else
        {
            activePlatform = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.y < -0.9 && collision.collider.HasTag(TagManager.manager.stand_platform))
        {
            if (activePlatform != collision.collider.transform)
            {
                activePlatform = collision.collider.transform;
                UpdateMovingPlatform();
            }
        }
        else
        {
            activePlatform = null;
        }
    }

    private void UpdateMovingPlatform()
    {
        activeGlobalPlatformPoint = transform.position;
        activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);

        // Support moving platform rotation
        activeGlobalPlatformRotation = transform.rotation;
        activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;
    }
}