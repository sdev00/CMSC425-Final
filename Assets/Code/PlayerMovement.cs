using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int sensitivity = 100;
    private int sensitivityAdjustment = 20;
    //private float playerSpeed = 0;
    private float playerAngleY = 0;
    private float playerAngleX = 0;

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {

        }

        playerAngleY += Input.GetAxis("Mouse X") * sensitivity / sensitivityAdjustment;
        playerAngleX = Mathf.Clamp(playerAngleX - Input.GetAxis("Mouse Y") * sensitivity / sensitivityAdjustment, -60, 60);

        transform.rotation = Quaternion.Euler(0, playerAngleY, 0) * Quaternion.Euler(playerAngleX, 0, 0);
    }
}
