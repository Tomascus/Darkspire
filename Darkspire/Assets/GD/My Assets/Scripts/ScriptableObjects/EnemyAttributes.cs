using UnityEngine;


// Scriptable object that holds values for different enemies
// Learned how to implement from: https://docs.unity3d.com/Manual/class-ScriptableObject.html

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "SO/Enemy/Enemy Attributes")]
public class EnemyAttributes : ScriptableObject
{
    public string enemyName;
    public float health;
    public float attackDamage;
    public float attackRange;
    public float movementSpeed;
    public float detectionRange;
    public int xpReward;
}
