using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    [Tooltip("Empty where object will get spawned.")]
    [SerializeField] private Transform spawningPoint;

    [Tooltip("Object to spawn from the chest.")]
    // [RequireComponent(typeof(IInstantiatePrefab))]
    [SerializeField] ScriptableObject item;

    private bool playerInRange = false;

    private void Awake()
    {
        if(spawningPoint == null)
        {
            Debug.LogError("Spawning Point is not set for the chest.");
        }

        if(item == null)
        {
            Debug.LogError("Item is not set for the chest.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) //when player is in range and presses E drop the item
        {
            GenerateChestItem();
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    //Generate the item, script based on Nial's code from class 
    private void GenerateChestItem()
    {
        if(item == null) Debug.LogError("Item is not set for the chest.");

        var generator = item as IInstantiatePrefab;
        if (generator == null) Debug.LogError("Item doesnt implement IInstantiatePrefab.");

        var newItem = generator.Instantiate(spawningPoint);
        if (newItem == null) Debug.LogError("Item failed to generated.");
    }

}