using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    // public GameObject stats;
    public GameObject player;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (paused)
            {
               Resume(); 
            } else 
            {
                Pause();
            }
        }

    }

    public void Resume() 
    {
        paused = false; 
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        player.GetComponent<PlayerHandling>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // stats.SetActive(true);
    }

    void Pause() 
    {
        paused = true; 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        player.GetComponent<PlayerHandling>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // stats.SetActive(false);
    }

    public void Options() 
    {
        Debug.Log("Loading menu");
        paused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        player.GetComponent<PlayerHandling>().enabled = true;
        SceneManager.LoadScene("Options");
    }

    public void Quit() 
    {
        Debug.Log("Quit");
        paused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        player.GetComponent<PlayerHandling>().enabled = true;
        SceneManager.LoadScene("Menu");
    }
}
