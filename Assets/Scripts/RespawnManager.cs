using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint; 
    public void RespawnPlayer(GameObject player)
    {
        // Move the player's transform to the respawn point
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;

        // Synchronize transforms to ensure changes are applied immediately
        Physics.SyncTransforms();
    }
    public Transform aiSpawnPoint; // Spawn point for AI

    public void RespawnAI(GameObject aiObject)
    {
        // Move AI to spawn point
        aiObject.transform.position = aiSpawnPoint.position;
        aiObject.transform.rotation = aiSpawnPoint.rotation;
        Physics.SyncTransforms();
        
    }
}
