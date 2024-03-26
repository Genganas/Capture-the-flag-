using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array of power-up prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points for the power-ups
    public float spawnInterval = 5f; // Time interval between spawns
    public float despawnDelay = 10f; // Time delay before despawning power-ups

    private void Start()
    {
        // Start spawning power-ups
        InvokeRepeating("SpawnPowerUp", 0f, spawnInterval);
    }

    private void SpawnPowerUp()
    {

        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        
        GameObject powerUp = Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);

        Destroy(powerUp, despawnDelay);
    }
}
