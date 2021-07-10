using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroFade : MonoBehaviour
{
    TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartBlinking();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            if (text.color.a.ToString() == "0") {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                yield return new WaitForSeconds(0.5f);
            } else {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void StartBlinking()
    {
        StopCoroutine("Blink");
        StartCoroutine("Blink");
    }
}