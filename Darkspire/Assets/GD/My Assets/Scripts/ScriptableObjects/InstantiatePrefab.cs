using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiatePrefab", menuName = "SO/Instantiate/Item")]

public class InstantiatePrefab : ScriptableObject, IInstantiatePrefab
{
    [Tooltip("Name of item to be instantiated.")]
    [SerializeField] private string description;

    [Tooltip("Prefab to be instantiated draged in here.")]
    [SerializeField] private GameObject prefab;

    [Tooltip("Is the object generated active?")]
    [SerializeField] private bool isGeneratedActive = true;

    public GameObject Instantiate(Transform transform)
    {
        var item = Instantiate(prefab, transform); //Instantiate the prefab at the transfrom location  

        item.SetActive(isGeneratedActive); // set it active 

        return item; //give back the created item 
    }


}
