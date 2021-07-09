using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenu : MonoBehaviour
{

    public Slider slider;
    public int value;
    TextMeshProUGUI txt;

    void Start ()
    {
        txt = GetComponent<TextMeshProUGUI>();
        slider.value = (float) PlayerMovement.sensitivity;
    }

    void Update ()
    {
        slider.value = value;
    }

    public void ShowValue() 
    {
        txt.text = "" + slider.value;
        value = (int) slider.value;
    }

    public void Sensitivity() 
    {
        PlayerMovement.sensitivity = value;
    }

    public void Back() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
