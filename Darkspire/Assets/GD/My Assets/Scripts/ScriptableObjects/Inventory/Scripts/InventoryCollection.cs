using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD.Items
{
    [CreateAssetMenu(fileName = "InventoryCollection", menuName = "GD/Inventory/Collection")]
    public class InventoryCollection : SerializedScriptableObject
    {
        #region Fields

        [SerializeField]
        [Tooltip("A list of all inventories (e.g. a saddlebag)")]
        private List<Inventory> contents = new List<Inventory>();

        [FoldoutGroup("Events")]
        [SerializeField]
        [Tooltip("Event to raise when the collection changes.")]
        private GameEvent onCollectionChange;

        [FoldoutGroup("Events")]
        [SerializeField]
        [Tooltip("Event to raise when the collection is emptied.")]
        private GameEvent onCollectionEmpty;

        #endregion Fields

        #region Properties

        public Inventory this[int index]
        {
            get
            {
                return contents[index];
            }
        }

        public int Count
        {
            get
            {
                return contents.Count;
            }
        }

        #endregion Properties

        public Inventory Get(int index)
        {
            if (index < 0 || index >= contents.Count)
                throw new IndexOutOfRangeException("No inventory at this index");

            return contents[index];
        }

        // Add a new inventory to the collection
        public void Add(Inventory inventory)
        {
            if (!contents.Contains(inventory))
            {
                contents.Add(inventory);
                Debug.Log("Added inventory to collection");
                if (onCollectionChange != null)
                {
                    onCollectionChange.Raise();
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
            if (contents.Contains(inventory))
            {
                contents.Remove(inventory);
                onCollectionChange?.Raise();
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