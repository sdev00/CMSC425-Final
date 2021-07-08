using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClearScene();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ClearScene()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }
}
