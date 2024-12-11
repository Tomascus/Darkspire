using GD.Items;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryCollection inventoryCollection; //what invetory collection ?  scriptable object 
    [SerializeField] private Inventory inventory; //for resetting inventory at start of the game
    private DroppedItem nearbyItem; //interactable items 
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        inventory.InitializeInventory();
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

    public void PickUpItem()
    {
        if (nearbyItem != null) //if the nearby item is not null
        {
            if (nearbyItem.ItemData != null) //and its data is not null
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
            SoundManager.PlaySound(SoundType.PLAYERPICKUP,audioSource);
        }
    }

    
}