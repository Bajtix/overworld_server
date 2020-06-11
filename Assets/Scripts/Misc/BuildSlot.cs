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

}
