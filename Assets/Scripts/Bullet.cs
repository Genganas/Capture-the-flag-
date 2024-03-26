using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit an AI
        AiHealth aiHealth = other.GetComponent<AiHealth>();
        if (aiHealth != null)
        {
            // Deal damage to the AI
            aiHealth.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
