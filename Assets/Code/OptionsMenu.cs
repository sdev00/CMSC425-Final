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
    public Button selectedButton;
    public static string difficulty = "EASY";

    void Start ()
    {
        txt = GetComponent<TextMeshProUGUI>();
        selectedButton.GetComponent<Image>().color = Color.black;
        slider.value = (float) PlayerMovement.sensitivity;
    }

    void Update ()
    {
        slider.value = value;
        selectedButton.GetComponent<Image>().color = Color.black;
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

    public void Radio(Button button)
    {
        if(selectedButton != null) 
            selectedButton.GetComponent<Image>().color = Color.clear;
        button.GetComponent<Image>().color = Color.black;
        selectedButton = button;
        difficulty = button.GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
