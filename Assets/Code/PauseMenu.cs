using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public GameObject stats;
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
        player.GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        stats.SetActive(true);
    }

    void Pause() 
    {
        paused = true; 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        player.GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        stats.SetActive(false);
    }

    public void Options() 
    {
        Debug.Log("Loading menu");
        SceneManager.LoadScene("Options");
    }

    public void Quit() 
    {
        Debug.Log("Quit");
        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                     // Application.Quit() does not work in the editor so
                     // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                     UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }
}
