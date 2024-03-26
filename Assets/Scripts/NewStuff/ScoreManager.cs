using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI blueScoreText;
    public TextMeshProUGUI redScoreText;

    private int blueScore = 0;
    private int redScore = 0;

    private void Start()
    {
    }
    private void Update()
    {
       
    }
    public void IncreaseScore(Flag.FlagType flagType)
    {
        if (flagType == Flag.FlagType.Blue)
        {
            blueScore++;
            blueScoreText.text = "Blue Team Score: " + blueScore;
          
        }
        
        if (flagType == Flag.FlagType.Red)
        {
            redScore++;
            redScoreText.text = "Red Team Score: " + redScore;
      
        }

        CheckForWin();
    }

    private void CheckForWin()
    {
        if (blueScore >= 5)
        {
            SceneManager.LoadScene("PlayerWinScene");
        }
        else if (redScore >= 5)
        {
            SceneManager.LoadScene("AIWinScene");
        }
    }
}


