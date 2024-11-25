using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;

    /*Explanation
     * Similar working as UI sciprt, on our inventory gameobject in our canvas we put gameEventListener where we put in scriptableobject game event:  InventoryChangeEvent 
     * and create response with dragging the inventory object there and adding InventortUIManager script and selecting OnInventoryChange method
     * This will update inventory UI when inventory changes
     * Then in playerInput  script we set the toggle on or off based on pressing key 
     */

    private void Start()
    {
        // Hide the inventory panel at the start
        inventoryPanel.SetActive(false);
        UpdateInventoryUI();
    }

    public void OnInventoryChange()
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        //clear existing slots to avoid duplicates
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        //iterate through players inventory items 
        foreach (KeyValuePair<ItemData, int> item in playerInventory)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer); //instantiate the item slot prefab with where to put it 
            ItemSlotUI itemSlotUI = itemSlot.GetComponent<ItemSlotUI>(); //get the item slot UI component
            itemSlotUI.SetItem(item.Key, item.Value); //set the item slot UI with the item and how many are in the inventory
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        UpdateInventoryUI(); //update the inventory UI when the panel is viewed
    }
}
