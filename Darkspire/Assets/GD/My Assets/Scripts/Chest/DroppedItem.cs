using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private ScriptableObject itemData;

    public ScriptableObject ItemData => itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.SetNearbyItem(this);
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
            }
            else
            {
                Debug.LogError("PlayerInventory component not found on the player.");
            }
        }
    }
}
