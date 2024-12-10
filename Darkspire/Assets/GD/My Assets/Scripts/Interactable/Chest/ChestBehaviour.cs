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

    public bool playerInRange = false;
    private bool itemGenerated = false; //for dropping only once 

    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
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
    public void GenerateChestItem()
    {
        if(item == null) Debug.LogError("Item is not set for the chest.");

        if(itemGenerated) return; //do not spawn multiples

        var generator = item as IInstantiatePrefab;
        if (generator == null) Debug.LogError("Item doesnt implement IInstantiatePrefab.");

        var newItem = generator.Instantiate(spawningPoint);
        if (newItem == null) Debug.LogError("Item failed to generated.");

        itemGenerated = true;
        SoundManager.PlaySound(SoundType.CHESTOPEN, audioSource);
    }

}
