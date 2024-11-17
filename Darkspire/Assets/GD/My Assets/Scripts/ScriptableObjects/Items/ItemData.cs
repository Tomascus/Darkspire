using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject, IInstantiatePrefab
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string description;
    [SerializeField] private GameObject prefab;


    public GameObject Instantiate(Transform spawnPoint) //forced method from interface
    {
        if (prefab == null) // check if prefab is set 
        {
            Debug.LogError("Prefab is not set for the item.");
            return null;
        }

        return Object.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation); //instantiate the prefab at the spawn point
    }

    //accessors
    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;
    public string Description => description;

}
