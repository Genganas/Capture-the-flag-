using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Text HealthText;
    public DamageSplash damageSplash; 
    public AudioSource healthSound;

    [SerializeField] private GameObject respawnPoint; 
    [SerializeField] private RespawnManager respawnManager; 

    private Flag flagScript; 
    void Start()
    {
        GameObject flagObject = GameObject.FindGameObjectWithTag("Flag");
        if (flagObject != null)
        {
            flagScript = flagObject.GetComponent<Flag>();
        }
        else
        {
           
        }
        currentHealth = maxHealth;
    }

    void Update()
    {
       
        if (currentHealth <= 0)
        {
            Die();
        }
        HealthText.text = currentHealth.ToString();
    }

   
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Show damage splash screen
        damageSplash.ShowDamage(damage);

        // Play health sound
        healthSound.Play();
    }


    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }


    void Die()
    {
       
        if (respawnManager != null)
        {
            respawnManager.RespawnPlayer(gameObject); // Pass the player GameObject as an argument
        }
        else
        {
           
        }

        // Reset health
        currentHealth = maxHealth;

      
        if (flagScript != null)
        {
            flagScript.ReturnToBase();
        }
        else
        {
           
        }
    }

   
    public void ApplyPowerUp()
    {
        healthSound.Play();
        IncreaseHealth(50);
    }
}
