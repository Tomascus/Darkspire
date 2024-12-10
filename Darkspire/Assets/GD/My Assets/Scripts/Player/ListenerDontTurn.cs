using UnityEngine;

public class ListenerDontTurn : MonoBehaviour   //Makes sure that the listener is facing camera, allowing the sound to play from the right direction
{
    public Transform cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
