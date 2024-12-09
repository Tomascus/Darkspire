using GD.Items;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryCollection inventoryCollection;
    private DroppedItem nearbyItem;

    private AudioSource audioSource;

    private void Update()
    {
        if (nearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetNearbyItem(DroppedItem item)
    {
        nearbyItem = item;
    }

    public void ClearNearbyItem(DroppedItem item)
    {
        if (nearbyItem == item)
        {
            nearbyItem = null;
        }
    }

    private void PickUpItem()
    {
        if (nearbyItem != null)
        {
            if (nearbyItem.ItemData != null)
            {
                // Cast ItemData to the correct type
                ItemData itemData = nearbyItem.ItemData as ItemData;
                if (itemData != null)
                {
                    try
                    {
                        // Assuming the first inventory in the collection is the player's inventory
                        Inventory playerInventory = inventoryCollection[0];
                        playerInventory.Add(itemData, 1);
                        Debug.Log($"Item added to inventory: {itemData.ItemName}");
                        
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                else
                {
                    Debug.LogError("ItemData is not of type ItemData.");
                }
            }
            else
            {
                Debug.LogError("ItemData is null.");
            }

            Destroy(nearbyItem.gameObject);
            nearbyItem = null;
            SoundManager.PlaySound(SoundType.PLAYERPICKUP, audioSource);
        }
    }
}