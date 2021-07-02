using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroids : MonoBehaviour
{
    public GameObject asteroid;
    private int count = 5000;
    private int minRange = -3000;
    private int maxRange = 3000;
    private float minSize = 2f;
    private float maxSize = 100f;
    private float sizeVariation = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(minRange, maxRange);
            int y = Random.Range(minRange, maxRange);
            int z = Random.Range(minRange, maxRange);
            Vector3 pos = new Vector3(x, y, z);
            GameObject clone = Instantiate(asteroid, pos, Quaternion.identity);
            clone.transform.localScale = (Random.value * (maxSize - minSize) + minSize) * 
                new Vector3(1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation, 1 + Random.value * sizeVariation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
