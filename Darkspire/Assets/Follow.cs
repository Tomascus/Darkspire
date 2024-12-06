using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 100 * Time.deltaTime);
    }
}
    
