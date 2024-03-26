using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedIncreaseAmount = 30f; 
    public float duration = 5f; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPSController fpsController = other.GetComponent<FPSController>();
            if (fpsController != null)
            {
                fpsController.ApplySpeedPowerUp(speedIncreaseAmount, duration);
                Destroy(gameObject); // Destroy the power-up object after the player collects it
            }
        }
    }
}

