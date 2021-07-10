using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI txt;
    public int value;

    void Start ()
    {
        txt = GetComponent<TextMeshProUGUI>();
        UpdateTime();
    }

    void Update() 
    {
        UpdateTime();
    }

    public void UpdateTime() 
    {
        txt.text = value + " s";
    }
}
