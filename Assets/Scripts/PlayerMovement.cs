using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Rigidbody Values")]

    private Rigidbody controller;
    private Transform groundChecker;
    private Vector3 moveVector;
    private Vector3 moveVectorAxis;
    public LayerMask Ground;

    private float timeToWaitUntilReload = 1f;
    private bool isDead = false;

    private float speed = 2f;
    private float verticalVelocity = 0.0f;
    public float GroundDistance = 0.2f;
    private bool isGrounded = true;

    [Header("Firing")]
    public KeyCode shootKey = KeyCode.Space;
    public GameObject projectilePrefab;
    public Transform projectileMount;

    [Header("Arduino Variables")]

    public Arduino arduino;   
    public int pin = 0;
    public float pinValue;
    public float mappedPot;
    public int pinButton = 6;
    public float pinButtonValue;

    [Header("Player Variables")]

    public float leftEdge;
    public float rightEdge;

    void Start()
    {
        arduino = Arduino.global;
        arduino.Log = (s) => Debug.Log("Arduino: " + s);
        arduino.Setup(ConfigurePins);

        controller = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        pinValue = arduino.analogRead(pin);
        mappedPot = pinValue.Remap(1023, 0, leftEdge, rightEdge);

        pinButtonValue = arduino.digitalRead(pinButton);

        moveVector = Vector3.zero;
        moveVectorAxis = Vector3.zero;

        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        //X - Left & Right 
        moveVector.x = mappedPot * speed;

        moveVectorAxis.x = Input.GetAxisRaw("Horizontal") * speed;

        //Y - Up & Down
        moveVector.y = verticalVelocity;

        moveVectorAxis.y = verticalVelocity;

        //Z - Forward & Backward
        moveVector.z = Input.GetAxisRaw("Vertical") * 2.0f;

        moveVectorAxis.z = Input.GetAxisRaw("Vertical") * 3.0f;

        if (pinButtonValue == 1)

        //if (Input.GetKeyDown(shootKey))
        {
            CmdFire();
        }

    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        //if(!isLocalPlayer) return;

        if (mappedPot < 1.5)
        {
           controller.MovePosition(controller.position + moveVector * speed * Time.fixedDeltaTime);
        }

        //controller.MovePosition(controller.position + moveVectorAxis * speed * Time.fixedDeltaTime);

    }

    void ConfigurePins()
    {
        arduino.pinMode(pin, PinMode.ANALOG);
        arduino.reportAnalog(pin, 1);

        arduino.pinMode(pinButton, PinMode.INPUT);
        arduino.reportDigital((byte)(pinButton / 8), 1);
        arduino.reportDigital((byte)(pinButton / 8), 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Death();
            Invoke("Reload", timeToWaitUntilReload);
        }
    }

    private void Death()
    {
        isDead = true;
    }

    private void Reload()
    {
        SceneManager.LoadScene("CarGame");
    }

    [Command]
    void CmdFire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
        NetworkServer.Spawn(projectile); 
    }
}
