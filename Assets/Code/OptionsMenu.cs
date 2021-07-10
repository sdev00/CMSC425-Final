using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public static GameObject sliderObject;
    private Slider slider;
    private int value = 50;

    public static GameObject textObject;
    private TextMeshProUGUI txt;

    public static GameObject selectedButtonObject;
    public Button selectedButton;

    public static string difficulty = "EASY";
    public static GameObject setupObject;

    void Start ()
    {
        sliderObject = GameObject.FindWithTag("sliderObject");
        textObject = GameObject.FindWithTag("textObject");
        selectedButtonObject = GameObject.FindWithTag("selectedButtonObject");
        setupObject = GameObject.FindWithTag("UserSetup");

        slider = sliderObject.GetComponent<Slider>();
        selectedButton = selectedButtonObject.GetComponent<Button>();
        txt = textObject.GetComponent<TextMeshProUGUI>();
        selectedButton.GetComponent<Image>().color = Color.black;

        slider.value = value;
        setupObject.GetComponent<TrackUserSetup>().sensitivity = value;
    }

    void Update ()
    {    }

    public void ShowValue() 
    {
        txt.text = "" + slider.value;
        value = (int) slider.value;
    }

    public void Sensitivity() 
    {
        setupObject.GetComponent<TrackUserSetup>().sensitivity = value;
    }

    public void RunGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Radio(Button button)
    {
        if (selectedButton != null) 
            selectedButton.GetComponent<Image>().color = Color.clear;

        selectedButton = button;
        selectedButton.GetComponent<Image>().color = Color.black;

        difficulty = button.GetComponentInChildren<TextMeshProUGUI>().text;

        Debug.Log("New difficulty: " + difficulty);
        switch (difficulty)
        {
            case "EASY":
                setupObject.GetComponent<TrackUserSetup>().difficulty = DifficultyLevel.Easy;
                break;
            case "MEDIUM":
                setupObject.GetComponent<TrackUserSetup>().difficulty = DifficultyLevel.Medium;
                break;
            case "HARD":
                setupObject.GetComponent<TrackUserSetup>().difficulty = DifficultyLevel.Hard;
                break;
        }
        Debug.Log("Difficulty is now: " + setupObject.GetComponent<TrackUserSetup>().difficulty);
    }
}
