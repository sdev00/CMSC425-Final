using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject camera, cameraChild;
    public int sensitivity = 100;
    public bool gameComplete = false;
    AudioSource audioSource;
    public GameObject progradeMarker, retrogradeMarker, thrustMarker;

    private float acceleration = 50;
    private float rotation = 60;
    private float abilityRecharge = 3;

    private float abilitySpeed = 1500;
    private int sensitivityAdjustment = 10;
    private float cameraAngleY = 0;
    private float cameraAngleX = 0;
    private float playerAngleY = 0;
    private float playerAngleX = 0;
    private float playerAngleZ = 0;
    private float playerRotationY = 0;
    private float playerRotationX = 0;
    private float playerRotationZ = 0;
    private float maxcameraAngleX = 90;
    private bool isThrusting = false;
    private float scrollStep = 20;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                     // Application.Quit() does not work in the editor so
                     // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                     UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRotationY -= Time.deltaTime * rotation;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRotationY += Time.deltaTime * rotation;
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerRotationX += Time.deltaTime * rotation;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRotationX -= Time.deltaTime * rotation;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            playerRotationZ += Time.deltaTime * rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            playerRotationZ -= Time.deltaTime * rotation;
        }

        bool appliedNewThrust = false;
        if (Input.GetKey(KeyCode.Z))
        {
            rb.velocity += Time.deltaTime * acceleration * transform.forward;
            appliedNewThrust = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            rb.velocity -= Time.deltaTime * acceleration * transform.forward;
            appliedNewThrust = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity += Time.deltaTime * acceleration * transform.up;
            appliedNewThrust = true;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity -= Time.deltaTime * acceleration * transform.up;
            appliedNewThrust = true;
        }
        
        if (appliedNewThrust) {
            if (!isThrusting) {
                audioSource.Play(0);
            }
            isThrusting = true;
        } else {
            if (isThrusting) {
                audioSource.Stop();
            }
            isThrusting = false;
        }

        playerAngleY = playerRotationY;
        playerAngleX = playerRotationX;
        playerAngleZ = playerRotationZ;
        
        rb.transform.rotation = Quaternion.Euler(0, playerAngleY, 0) * 
                Quaternion.Euler(playerAngleX, 0, 0) *
                Quaternion.Euler(0, 0, playerAngleZ);

        // cameraAngleY += playerRotationY;
        // cameraAngleX += playerRotationX;
        // playerRotationX = playerRotationY = playerRotationZ = 0;
        cameraAngleY += Input.GetAxis("Mouse X") * sensitivity / sensitivityAdjustment;
        cameraAngleX = Mathf.Clamp(cameraAngleX - Input.GetAxis("Mouse Y") * sensitivity / sensitivityAdjustment, -maxcameraAngleX, maxcameraAngleX);

        
        cameraChild.transform.position = Vector3.MoveTowards(cameraChild.transform.position, camera.transform.position, Input.GetAxis("Mouse ScrollWheel") * scrollStep);
        camera.transform.rotation = Quaternion.Euler(0, cameraAngleY, 0) * Quaternion.Euler(cameraAngleX, 0, 0);

        progradeMarker.transform.rotation = Quaternion.LookRotation(rb.velocity);
        retrogradeMarker.transform.rotation = progradeMarker.transform.rotation * Quaternion.Euler(0, 180, 0);
    }
}
