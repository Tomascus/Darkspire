using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD.Items
{
    [CreateAssetMenu(fileName = "InventoryCollection", menuName = "GD/Inventory/Collection")]
    public class InventoryCollection : SerializedScriptableObject //SerializedScriptableObject allows us to see what is happeing in the inventory
    {
        /*MAIN DUTY OF THIS SCRIPT IS TO CREATE A SCRIPTABLE OBJECT THAT MANAGES COLLECTION OF INVENTORIES (INVENTORY OBJECTS)
         * WITH METHODS TO ADD, REMOVE AND CLEAR THE INVENTORIES PLUS RAISE EVENTS WHEN SOMETHING IN THE COLLECTION CHANGES
         * THIS SCRIPTABLE OBJECT WILL TAKE IN ANOTHER SCRIPTABLE OBJECT WHICH IS INVENTORY 
         */

        #region Fields

        [SerializeField]
        [Tooltip("A list of all inventories (e.g. a saddlebag)")]
        private List<Inventory> contents = new List<Inventory>();

        [FoldoutGroup("Events")]
        [SerializeField]
        [Tooltip("Event to raise when the collection changes.")]
        private GameEvent onCollectionChange; //event raise when for example an inventory is added or removed

        [FoldoutGroup("Events")]
        [SerializeField]
        [Tooltip("Event to raise when the collection is emptied.")]
        private GameEvent onCollectionEmpty; //when collection is cleared notice others

        #endregion Fields

        #region Properties

        public Inventory this[int index] //This is an indexer whivh allows access to the inventory at specific index 
        {
            get
            {
                return contents[index]; 
            }
        }

        public int Count //returns number of inventories in the collection
        {
            get
            {
                return contents.Count;
            }
        }

        #endregion Properties

        public Inventory Get(int index) //getter for inventory at specific index
        {
            if (index < 0 || index >= contents.Count)
                throw new IndexOutOfRangeException("No inventory at this index");

            return contents[index];
        }

        // Add a new inventory to the collection of inventories
        public void Add(Inventory inventory)
        {
            if (!contents.Contains(inventory)) //if inventory is not already in the collection
            {
                contents.Add(inventory); //add new one 
                Debug.Log("Added inventory to collection");
                if (onCollectionChange != null)
                {
                    onCollectionChange.Raise(); //raise event to notify others that collection has changed
                    Debug.Log("onCollectionChange event raised");
                }
                else
                {
                    Debug.LogWarning("onCollectionChange event is null");
                }
            }
        }

        // Removes an inventory from the collection
        public void Remove(Inventory inventory)
        {
            if (contents.Contains(inventory)) //if not empty
            {
                contents.Remove(inventory); //remove inventory 
                onCollectionChange?.Raise(); //notify subscribers 
            }
        }

        // Removes all inventories from the collection
        public bool Clear()
        {
            contents.Clear();
            onCollectionEmpty?.Raise();
            return contents.Count == 0;
        }
    }
}