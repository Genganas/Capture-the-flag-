using UnityEngine;

public class AiHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public AudioSource damageSound;
    private Flag flagScript;

    private RespawnManager respawnManager; // Reference to the RespawnManager script

    private void Start()
    {
        currentHealth = maxHealth;

        // Get reference to RespawnManager script
        respawnManager = GameObject.FindGameObjectWithTag("RespawnManager").GetComponent<RespawnManager>();
        if (respawnManager == null)
        {
            
        }

        // Get reference to Flag script
        GameObject flagObject = GameObject.FindGameObjectWithTag("RedFlag");
        if (flagObject != null)
        {
            flagScript = flagObject.GetComponent<Flag>();
        }
        else
        {
          
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageSound.Play();
        if (currentHealth <= 0)
        {
            flagScript.ReturnToBase();
            Die();
        }
    }

    private void Die()
    {
        // Call respawn logic from RespawnManager
        if (respawnManager != null)
        {
            respawnManager.RespawnAI(gameObject);
           
        }
        else
        {
          
        }

        
    }
}
