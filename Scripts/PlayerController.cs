using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private AudioSource audioSource;
    public float speed = 1.0f;
    public float tilt;
    public Boundary boundary;

    public GameObject[] shots;
    public Transform[] shotSpawnsS;
    public Transform[] shotSpawnsD;
    public Transform[] shotSpawnsT;

    public float fireRate;
    private float nextFire;

    public SimpleTouchPad touchPad;
    public SimpleTouchAreaButton areaButton;
    private Quaternion calibrationQuaternion;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        CalibrateAccelerometer();
    }

    void Update()
    {
        if(areaButton.CanFire() && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (var shotSpawn in shotSpawnsS)
            {
                Instantiate(shots[0], shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }

    }

    void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //Vector3 accelerationRaw = Input.acceleration;
        //Vector3 acceleration = FixAcceleration(accelerationRaw);
        //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);

        Vector2 direction = touchPad.GetDirection();
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
        rigidbody.velocity = movement * speed;
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
        );
        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
    }
    void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (var shotSpawn in shotSpawnsS)
            {
                Instantiate(shots[0], shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }
        if (Input.GetButton("Fire3") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (var shotSpawn in shotSpawnsD)
            {
                Instantiate(shots[1], shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }
        if (Input.GetButton("AltFire") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (var shotSpawn in shotSpawnsT)
            {
                Instantiate(shots[2], shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }
    }

    //Used to calibrate the Iput.acceleration input
    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    //Get the 'calibrated' value from the Input
    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }
}
