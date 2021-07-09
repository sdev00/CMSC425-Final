using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene("Game");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options() 
    {
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
