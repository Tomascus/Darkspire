using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUI.OnTakeDamage(5);
        }
    }
}

