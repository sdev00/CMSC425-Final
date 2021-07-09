using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private float speedToDestroy = 250f;
    private float massToDestroy = 100f;
    private float pieceCount = 2f;
    private float minSpeed = 5;
    private float minSize = 0.1f;
    private float maxSize = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        float mass = collision.gameObject.GetComponent<Rigidbody>().mass;
        float speed = collision.relativeVelocity.magnitude;
        Debug.Log("speed=" + speed);
        Debug.Log("mass=" + mass);
        Debug.Log(speed >= speedToDestroy);
        if (speed >= speedToDestroy && mass >= massToDestroy)
        {
            Debug.Log("destroyed");
            for (int i = 0; i < pieceCount; i++)
            {
                Debug.Log("generating asteroid");
                //GenerateAsteroids.summonAsteroid(transform.position,
                //    (minSpeed + Random.value * (speed - minSpeed)) * Random.rotation.eulerAngles, 
                //    minSize + Random.value * (maxSize - minSize));
            }
            Destroy(gameObject);
        }
    }
}
