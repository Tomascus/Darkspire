using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    #region FIELDS
    [Header("Item Data from scriptable object")]
    [SerializeField] private ScriptableObject itemData;

    [Header("UI to show when player in range")]
    [SerializeField] private GameObject pickupUI;
    #endregion

    public ScriptableObject ItemData => itemData;

    private void Awake()
    {
        if(itemData != null)
        {
            pickupUI.SetActive(false); //dont show
        }
    }

    #region COLLISION
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.SetNearbyItem(this);

                if(pickupUI != null)
                {
                    pickupUI.SetActive(true); //show when player collide
                }
            }
            else
            {
                Debug.LogError("PlayerInventory component not found on the player.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.ClearNearbyItem(this);

                if (pickupUI != null)
                {
                    pickupUI.SetActive(false); //dont show when player exit
                }
            }
            else
            {
                Debug.LogError("PlayerInventory component not found on the player.");
            }
        }
    }
}
#endregion
