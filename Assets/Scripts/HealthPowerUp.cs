using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHealthPowerUp : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ApplyPowerUp();
                Destroy(gameObject); 
            }
        }
    }
}
