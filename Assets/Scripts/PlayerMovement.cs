using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Rigidbody Values")]

    private Rigidbody controller;
    private Transform groundChecker;
    private Vector3 moveVector;
    public LayerMask Ground;

    private float timeToWaitUntilReload = 1f;
    private bool isDead = false;

    private float speed = 2f;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    public float GroundDistance = 0.2f;
    private bool isGrounded = true;

    [Header("Arduino Variables")]

    public Arduino arduino;   
    public int pin = 0;
    public float pinValue;
    public float mappedPot;

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

        moveVector = Vector3.zero;

        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        //X - Left & Right 
        moveVector.x = mappedPot * speed;

        //Y - Up & Down
        moveVector.y = verticalVelocity;

        //Z - Forward & Backward
        moveVector.z = speed;
    }

        private void FixedUpdate()
        {
            if (isDead)
                return;

            if (mappedPot < 1.5)
            controller.MovePosition(controller.position + moveVector * speed * Time.fixedDeltaTime);
        }

    void ConfigurePins()
    {
        arduino.pinMode(pin, PinMode.ANALOG);
        arduino.reportAnalog(pin, 1);
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

}
