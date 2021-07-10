using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hydrogen : MonoBehaviour
{
    TextMeshProUGUI txt;
    public int value = 2;

    void Start ()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateResources() 
    {
        txt.text = "" + value;
    }
}
