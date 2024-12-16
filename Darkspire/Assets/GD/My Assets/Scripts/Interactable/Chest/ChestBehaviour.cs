using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Empty where object will get spawned.")]
    [SerializeField] private Transform spawningPoint;

    [Tooltip("Object to spawn from the chest.")]
    // [RequireComponent(typeof(IInstantiatePrefab))]
    [SerializeField] ScriptableObject item;

    public bool playerInRange = false;
    private bool itemGenerated = false; //for dropping only once 

    private AudioSource audioSource;
    #endregion

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

    #region COLLISION
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
    #endregion

    #region GENERATE
    //Generate the item, script based on Nial's code from class 
    public void GenerateChestItem()
    {
        if(item == null) Debug.LogError("Item is not set for the chest.");

        if(itemGenerated) return; //do not spawn multiples

        var generator = item as IInstantiatePrefab;
        if (generator == null) Debug.LogError("Item doesnt implement IInstantiatePrefab.");

        var newItem = generator.Instantiate(spawningPoint);
        if (newItem == null) Debug.LogError("Item failed to generated.");

        itemGenerated = true; //already generate, do not do duplicates  
        SoundManager.PlaySound(SoundType.CHEST_OPEN, audioSource);
    }
}
#endregion
