using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    [SerializeField] GameObject panel1;
    [SerializeField] GameObject panel2;
    [SerializeField] GameObject panel3;

    public float delayBeforeSound = 1.0f;

    public AudioSource audioSource; // Reference to the AudioSource component

    public void GameplayScene()  //Loads the Game or scene which the game will start with
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OptionsScene()// Loads the Options scene
    {
        SceneManager.LoadScene("OptionsMenu");
    }
    public void MainMenu() // Loads the menu scene
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
    public void Dialogue() // Loads the Credits scene 
    {

        if (audioSource != null)
        {
            audioSource.Play();
        }

        StartCoroutine(PlaySoundWithDelay());

    }
    public void ReStart() // restarts scene 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NewGameScene");

    }
    IEnumerator PlaySoundWithDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayBeforeSound);
        SceneManager.LoadScene("Dialogue");
        // Play the sound if AudioSource is assigned

    }
    public void Quit() // Quits the game 
    {
        Application.Quit();
    }
    IEnumerator PlaySoundWithDelay2()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayBeforeSound);
        Application.Quit();
        // Play the sound if AudioSource is assigned

    }



    public void Settings() // Loads the checkpoint race
    {
        panel1.SetActive(false);
        panel2.SetActive(true);

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void SettingsMenu() // Loads the checkpoint race
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void Pause() // Loads the checkpoint race
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }
    public void Returnpause() // Loads the checkpoint race
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }
    public void BeginnerRace() // Loads the advanced race
    {
        SceneManager.LoadScene("BeginnerRace");
    }
    public void CHeckpointDialogue() // Loads the chekpoint race dialogue
    {
        SceneManager.LoadScene("CheckPointDialogue");
    }
    public void AdvancedRace() // Loads the Advanced race 
    {
        SceneManager.LoadScene("Advanced Race");
    }
    public void BeginnerDialogue() // Loads the beginner race dialogue
    {
        SceneManager.LoadScene("BeginnerRaceDialogue");
    }

    public void AdvancedDialogue() // Loads the advanced race dialogue
    {
        SceneManager.LoadScene("AdvancedRaceDialogue");
    }

    public void SetfullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ReturnGuide() // Loads the checkpoint race
    {
        panel3.SetActive(false);
        panel1.SetActive(true);
    }
    public void Guide() // Loads the checkpoint race
    {
        panel3.SetActive(true);
        panel1.SetActive(false);
    }
}