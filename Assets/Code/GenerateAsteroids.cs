using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    public GameObject asteroid;
    public int count = 500;
    public int minRange = -1000;
    public int maxRange = 1000;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(minRange, maxRange);
            int y = Random.Range(minRange, maxRange);
            int z = Random.Range(minRange, maxRange);
            Vector3 pos = new Vector3(x, y, z);
            Instantiate(asteroid, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
