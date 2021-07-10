using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHandling : MonoBehaviour
{
    public GameObject camera, cameraChild;
    public int sensitivity = 100;
    public bool gameComplete = false;
    public int health;
    public ResourceData resources = ResourceData.emptyResourceData();
    public AudioClip collisionSound, miningSound;
    AudioSource audioSource;
    public GameObject progradeMarker, retrogradeMarker, thrustMarker;
    public GameObject rearRightThrustFlame, rearLeftThrustFlame;

    public float maxTime;
    public float endTime;
    private float acceleration = 50;
    private float rotationSpeed = 60;
    private float invincibleUntil = 0;
    private float invincibilityPeriod = 2;

    private float maxMineableSpeed;
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
    private float scrollStep = 10;

    private Rigidbody rb;
    private GenerateAsteroids asteroidGen;

    public void difficultySetup(DifficultyLevel diffLevel)
    {
        Difficulty d = new Difficulty();
        switch (diffLevel)
        {
            case DifficultyLevel.Easy:
                d.health = 10;
                d.timeSeconds = 600;
                d.asteroidSpread = 1;
                d.asteroidYieldMultiplier = 2;
                d.maxMineableSpeed = 150;
                break;

            case DifficultyLevel.Medium:
                d.health = 5;
                d.timeSeconds = 360;
                d.asteroidSpread = 8;
                d.asteroidYieldMultiplier = 1;
                d.maxMineableSpeed = 50;
                break;

            case DifficultyLevel.Hard:
                d.health = 2;
                d.timeSeconds = 600;
                d.asteroidSpread = 60;
                d.asteroidYieldMultiplier = 0.75f;
                d.maxMineableSpeed = 8;
                break;
        }

        health = d.health;
        maxTime = d.timeSeconds;
        endTime = Time.time + d.timeSeconds;
        asteroidGen = GetComponent<GenerateAsteroids>();
        asteroidGen.asteroidsAdjustment = d.asteroidSpread;
        asteroidGen.resourcesAdjustment = d.asteroidYieldMultiplier;
        maxMineableSpeed = d.maxMineableSpeed;
    }

    public void runGame(DifficultyLevel diffLevel)
    {
        difficultySetup(diffLevel);
        asteroidGen.generateAllAsteroids();
        StartCoroutine("Movement");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();

        runGame(DifficultyLevel.Hard);
    }

    IEnumerator Movement()
    {
        while (!gameComplete)
        {
            if (Time.time > endTime)
            {
                gameComplete = true;
                yield break;
            }

            if (Input.GetKey(KeyCode.A))
            {
                playerRotationY -= Time.deltaTime * rotationSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                playerRotationY += Time.deltaTime * rotationSpeed;
            }
            if (Input.GetKey(KeyCode.W))
            {
                playerRotationX += Time.deltaTime * rotationSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerRotationX -= Time.deltaTime * rotationSpeed;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                playerRotationZ += Time.deltaTime * rotationSpeed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                playerRotationZ -= Time.deltaTime * rotationSpeed;
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
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity += Time.deltaTime * acceleration * transform.up;
                appliedNewThrust = true;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.velocity -= Time.deltaTime * acceleration * transform.up;
                appliedNewThrust = true;
            }

            if (appliedNewThrust)
            {
                if (!isThrusting)
                {
                    audioSource.Play(0);
                }
                isThrusting = true;
            }
            else
            {
                if (isThrusting)
                {
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

            if (rb.velocity != Vector3.zero)
            {
                progradeMarker.transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
            retrogradeMarker.transform.rotation = progradeMarker.transform.rotation * Quaternion.Euler(0, 180, 0);

            yield return null;
        }
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

        if (gameComplete)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            playerRotationY -= Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRotationY += Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerRotationX += Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRotationX -= Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            playerRotationZ += Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            playerRotationZ -= Time.deltaTime * rotationSpeed;
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
        if (Input.GetKey(KeyCode.Space))
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

        if (isThrusting) {
            Vector3 toCamera;
            rearRightThrustFlame.SetActive(true);
            toCamera = cameraChild.transform.position - rearRightThrustFlame.transform.position;
            rearRightThrustFlame.transform.LookAt(rearRightThrustFlame.transform.position + rearRightThrustFlame.transform.forward, toCamera);
        
            rearLeftThrustFlame.SetActive(true);
            toCamera = cameraChild.transform.position - rearLeftThrustFlame.transform.position;
            rearLeftThrustFlame.transform.LookAt(rearLeftThrustFlame.transform.position + rearLeftThrustFlame.transform.forward, toCamera);

        } else {
            rearRightThrustFlame.SetActive(false);
            rearLeftThrustFlame.SetActive(false);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Marker")
            return;

        if (collision.relativeVelocity.magnitude > maxMineableSpeed && Time.time > invincibleUntil)
        {
            invincibleUntil = Time.time + invincibilityPeriod;
            audioSource.PlayOneShot(collisionSound);
            health -= 1;
            if (health < 1)
            {
                gameComplete = true;
                audioSource.Stop();
            }
        }
        else
        {
            AsteroidData ad = collision.gameObject.GetComponent<AsteroidData>();
            if (ad.resources.getTotal() > 0)
            {
                resources += ad.resources;
                audioSource.PlayOneShot(miningSound);
                ad.resources = ResourceData.emptyResourceData();
            }
        }
    }
}

public struct Difficulty
{
    public int health;
    public float timeSeconds;
    public float asteroidSpread;
    public float asteroidYieldMultiplier;
    public float maxMineableSpeed;
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}