using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "GD/Inventory/Inventory")]
public class Inventory : SerializedScriptableObject, IEnumerable<KeyValuePair<ItemData, int>>
{
    /*THIS SCRIPT ALLOWS TO CREATE A SCRIPTABLE OBJECT INVENTORY WHICH ALLOWS US TO MANAGE COLLECTION OF ITEMS
     *AND COUNTS OF THE ITEMS, PROVIDES METHOD ADD, REMOVE AND CLEAR WITH RAISING EVENTS ON CHANGE OF INVENTORY 
     *  NIAL'S CODE LOGIC FROM CLASS FOR INVENTORY MANAGEMENT
     */

    #region Fields

    [SerializeField]
    [Tooltip("The items in the inventory and their counts.")]
    private Dictionary<ItemData, int> contents = new Dictionary<ItemData, int>();

    [SerializeField]
    [Tooltip("Event to raise when the inventory changes.")]
    private GameEvent onInventoryChange;

    [SerializeField]
    [Tooltip("Event to raise when the inventory is cleared.")]
    private GameEvent onInventoryClear;

    #endregion Fields

    #region Properties

    public int this[ItemData itemData] //indexer that gets the count of the item in the inventory, takes in itemData which is another scriptable object itemData.cs
    {
        get
        {
            return contents[itemData];
        }
    }

    #endregion Properties

    /// <summary>
    /// Adds the specified amount of items to the inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void Add(ItemData item, int count)
    {
        if (contents.ContainsKey(item))
            contents[item] += count;
        else
            contents.Add(item, count);

        onInventoryChange?.Raise(); // Tell interested parties that the inventory has changed

    }

    /// <summary>
    /// Removes the specified amount of items from the inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int Remove(ItemData item, int count)
    {
        int remaining = 0;

        if (contents.ContainsKey(item))
        {
            if (contents[item] > count)
            {
                contents[item] -= count;
                remaining = contents[item];
            }
            else
            {
                contents.Remove(item);
            }
            onInventoryChange?.Raise(); // Tell interested parties that the inventory has changed
        }
        return remaining;
    }

    /// <summary>
    /// Gets the count of the specified item in the inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Count(ItemData item)
    {
        if (contents.ContainsKey(item))
            return contents[item];
        return 0;
    }

    /// <summary>
    /// Returns true if the inventory contains the specified item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(ItemData item)
    {
        return contents.ContainsKey(item);
    }

    /// <summary>
    /// Removes all items from the inventory.
    /// </summary>
    /// <returns></returns>
    public bool Clear()
    {
        contents.Clear();
        onInventoryClear?.Raise();
        onInventoryChange?.Raise();
        return contents.Count == 0;
    }

    [ContextMenu("Raise OnChange")]
    public void RaiseOnChange()
    {
        onInventoryChange?.Raise();
    }

    #region Methods - IEnumerable Implementation
    //these methods are used to implement IEnumerable interface to allow us to iterate over the inventory
    public IEnumerator<KeyValuePair<ItemData, int>> GetEnumerator()
    {
        return contents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion Methods - IEnumerable Implementation
}