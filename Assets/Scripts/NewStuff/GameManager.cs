using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blueBase;
    public GameObject redBase;
    public GameObject flagPrefab;
    public ScoreManager scoreManager;

    private Flag blueFlag;
    private Flag redFlag;

    private void Start()
    {
        // Spawn flags at bases
        blueFlag = SpawnFlag(blueBase.transform.position, Flag.FlagType.Blue);
        redFlag = SpawnFlag(redBase.transform.position, Flag.FlagType.Red);
    }

    private Flag SpawnFlag(Vector3 position, Flag.FlagType flagType)
    {
        GameObject flagObject = Instantiate(flagPrefab, position, Quaternion.identity);
        Flag flag = flagObject.GetComponent<Flag>();
        flag.flagType = flagType;
        return flag;
    }

    // Method to reset the flags to their bases
  

    // Method to handle flag pickup by players and AI
    // Method to handle flag pickup by players and AI
    public void FlagInteracted(Flag flag, GameObject entity)
    {
        if (entity.CompareTag("Player") || entity.CompareTag("AI"))
        {
            // Check if the flag's base is the same as the entity's base
            if (flag.flagType == Flag.FlagType.Red && flag.transform.position == redBase.transform.position)
            {
                // Flag picked up by player or AI and it's at the red base
                flag.transform.SetParent(entity.transform); // Parent the flag to the player or AI
                flag.gameObject.SetActive(false); // Hide the flag
                scoreManager.IncreaseScore(Flag.FlagType.Red); // Increase score for red team
            }
            else if (flag.flagType == Flag.FlagType.Blue && flag.transform.position == blueBase.transform.position)
            {
                // Flag picked up by player or AI and it's at the blue base
                flag.transform.SetParent(entity.transform); // Parent the flag to the player or AI
                flag.gameObject.SetActive(false); // Hide the flag
                scoreManager.IncreaseScore(Flag.FlagType.Blue); // Increase score for blue team
            }
        }
    }

}
