using UnityEngine;

public class Flag : MonoBehaviour
{
    public enum FlagType { Red, Blue };

    public FlagType flagType;
    private Vector3 initialPosition;
    private bool isCaptured = false;
    private ScoreManager scoreManager;

    private void Start()
    {
        initialPosition = transform.position;
        GameObject scoreManagerObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if (scoreManagerObject != null)
        {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        }
        else
        {
          
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isCaptured)
        {
            if (other.CompareTag("Player") && flagType == FlagType.Blue)
            {
               
                CaptureFlag(other.gameObject);
            }
            else if (other.CompareTag("AI") && flagType == FlagType.Red)
            {
               
                CaptureFlag(other.gameObject);
            }
        }
        
        {
            if (other.CompareTag("BlueBase") && flagType == FlagType.Blue)
            {
               
                scoreManager.IncreaseScore(flagType);
                ReturnToBase();
            }
            else if (other.CompareTag("RedBase") && flagType == FlagType.Red)
            {
                
                scoreManager.IncreaseScore(flagType);
                ReturnToBase();
            }
        }

       // Debug.Log("Flag Type: " + flagType + ", Is Captured: " + isCaptured);
        //Debug.Log("Collided with: " + other.gameObject.name);
    }

    private void CaptureFlag(GameObject capturer)
    {
        isCaptured = true;
        transform.SetParent(capturer.transform);
    }

    public void ReturnToBase()
    {
        isCaptured = false;
        transform.SetParent(null);
        transform.position = initialPosition;
    }
}
