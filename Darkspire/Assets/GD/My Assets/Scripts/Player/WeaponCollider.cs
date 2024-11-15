using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int attackDamage = 10; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Checks if the object hit is an enemy
        {

            // Gets the EnemyController component of hit object to call the TakeDamage method on the enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage); // Call the enemy's TakeDamage method
            }
        }
    }
}
