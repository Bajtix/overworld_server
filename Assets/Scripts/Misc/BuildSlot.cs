using UnityEngine;

public class BuildSlot : MonoBehaviour
{
    [System.Serializable]
    public enum PlaceSlotType
    {
        Wall = 1,
        Floor,
        Main,
        Foundation,
        Occupied
    }

    public PlaceSlotType type;


    private void Start()
    {
        /*if (type != PlaceSlotType.Occupied)
        {
            GetComponent<BoxCollider>().enabled = false;
            if (Physics.OverlapBox(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size / 2 - new Vector3(0.1f, 0.1f, 0.1f), transform.rotation, LayerMask.GetMask("Building", "BuildSlot Foundation", "BuildSlot Wall", "BuildSlot Floor", "BuildSlot Main")).Length > 0)
                gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = true;
        }*/
    }

}
