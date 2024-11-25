using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;

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
        descriptionPanel.SetActive(false);
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

            Button button = itemSlot.GetComponent<Button>(); //get the button component of the item slot
            if(button != null)
            {
                button.onClick.AddListener(() => OnItemClick(item.Key)); //add a listener to the button to call OnItemClick when clicked
            }
        }
    }

    public void ToggleInventoryPanel()
    {
        bool inInventory = !inventoryPanel.activeSelf; //check if the inventory panel is active or not
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        UpdateInventoryUI(); //update the inventory UI when the panel is viewed

        if (!inInventory)
        {
            descriptionPanel.SetActive(false); //if we close the inventory panel then close the description panel
        }
    }

    public void OnItemClick(ItemData item) //when we click on item (button) then show the panel
    {
        if(descriptionPanel != null && descriptionText != null)
        {
            descriptionText.text = item.Description; //set the description text to the item description
            descriptionPanel.SetActive(true); //show the description panel
        }
    }

    public bool IsInventoryPanelActive() //check for in inventory state 
    {
        return inventoryPanel.activeSelf;
    }
}
