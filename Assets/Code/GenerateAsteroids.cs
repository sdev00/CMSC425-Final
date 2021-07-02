using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    public GameObject asteroid;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(-200, 200);
            int y = Random.Range(-200, 200);
            int z = Random.Range(-200, 200);
            Vector3 pos = new Vector3(x, y, z);
            Instantiate(asteroid, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
