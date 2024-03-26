using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseButton : MonoBehaviour
{
    public bool IsPaused = false;
    public GameObject PauseMenu;
   

   
    // public GameObject timer;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Pause();
        }
        else
        {
            Time.timeScale = 1f;
            Resume();
        }
    }

    public void Resume()
    {
        IsPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
       

       
    }

    public void ResumeGame()
    {
       
        Time.timeScale = 1f;
        Cursor.visible = false; // Make the cursor visible
        Cursor.lockState = CursorLockMode.Locked;
        //timer.SetActive(true);
    }

    public void Pause()
    {
        IsPaused = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true; // Make the cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        // timer.SetActive(false);
        //settings.SetActive(false);
    }

    public void Settings()
    {
        IsPaused = true;
        PauseMenu.SetActive(false);
        Time.timeScale = 0f;
        // settings.SetActive(true);

    }
    public void Menu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        // Reloads the current scene, effectively restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}