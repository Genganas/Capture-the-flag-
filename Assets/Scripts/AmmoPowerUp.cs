using UnityEngine;

public class AmmoPowerUp : MonoBehaviour
{
    private FPSController fpsController; 

    private void Start()
    {
       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            fpsController = player.GetComponent<FPSController>();
            if (fpsController == null)
            {
               
            }
        }
        else
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fpsController != null)
            {
                fpsController.ReloadAmmo(); // Reload ammo when player collides with the power-up
                Destroy(gameObject); // Destroy the power-up object after the player collects it
            }
        }
    }
}

