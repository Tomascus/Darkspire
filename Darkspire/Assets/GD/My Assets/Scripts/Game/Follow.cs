using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    
    //Created for 3D sounds
    private void Update()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.y += 10; // Adjust the target position to be 10 units away on the z-axis
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 100 * Time.deltaTime);
    }

}

