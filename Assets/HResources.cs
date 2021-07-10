using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HResources : MonoBehaviour
{
    TextMeshProUGUI txt;
    public int value = CelestialBody.getH();

    void Start ()
    {
        txt = GetComponent<TextMeshProUGUI>();
        UpdateResources();
    }

    void Update() 
    {
        UpdateResources();
    }

    public void UpdateResources() 
    {
        txt.text = "" + value;
    }
}
