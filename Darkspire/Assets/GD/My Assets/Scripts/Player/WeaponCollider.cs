using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    // This HashSet is used to keep track of enemies hit by the weapon to prevent multiple hits on the same enemy - efficient for large numbers of enemies
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private bool isAttacking = false;

    [Header("Damage Settings")]
    [SerializeField] private playerAttributes playerAttributes;

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject)) // Checks if the object hit is an enemy and if it has already been hit by the weapon
        {
            hitEnemies.Add(other.gameObject);
            DealDamage(other.gameObject);
        }
    }

    // Temporary method to set the attacking state of the weapon in the PlayerCombatController script - allows to communicate attack state between scripts
    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    // This method is called from the PlayerCombatController script to reset the list of enemies hit by the weapon
    public void ResetHits()
    {
        hitEnemies.Clear();
    }

    private void DealDamage(GameObject enemy)
    {
        // Gets the EnemyController component of hit object to call the TakeDamage method on the enemy
        if (enemy != null)
        { 
        enemy.GetComponent<EnemyController>().TakeDamage(playerAttributes.currentStrength);
        }
    }
}
