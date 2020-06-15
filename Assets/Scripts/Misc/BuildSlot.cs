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
        if (Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size/2 -new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity,LayerMask.GetMask("Building")).Length > 0)
            gameObject.SetActive(false);
    }

}
