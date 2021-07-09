using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static int sensitivity = 50;
    private float acceleration = 150;
    private float abilityRecharge = 3;
    private float abilitySpeed = 1500;
    private int sensitivityAdjustment = 20;
    private float playerAngleY = 0;
    private float playerAngleX = 0;
    private float maxPlayerAngleX = 90;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.Escape))
        // {
        //     #if UNITY_EDITOR
        //              // Application.Quit() does not work in the editor so
        //              // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        //              UnityEditor.EditorApplication.isPlaying = false;
        //     #else
        //                 Application.Quit();
        //     #endif
        // }
        
        playerAngleY += Input.GetAxis("Mouse X") * sensitivity / sensitivityAdjustment;
        playerAngleX = Mathf.Clamp(playerAngleX - Input.GetAxis("Mouse Y") * sensitivity / sensitivityAdjustment, -maxPlayerAngleX, maxPlayerAngleX);

        transform.rotation = Quaternion.Euler(0, playerAngleY, 0) * Quaternion.Euler(playerAngleX, 0, 0);


        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += Time.deltaTime * acceleration * transform.forward;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = abilitySpeed * transform.forward;
        }
    }
}
