using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{

    public AudioSource audioSource;
    
    private void OnCollisionEnter(Collision other)
    {
        SoundManager.PlaySound(SoundType.HITWALL);
    }


}
