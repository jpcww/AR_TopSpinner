using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public Joystick joystick;
    public float speed = 2;
    private Vector3 velocityVector = Vector3.zero; // initial Velocity

    public float maxVelocityChange = 4f;
    private Rigidbody rb;

    public float tiltAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Taking the joystikc inputs
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        // Calculating velocity vectors for 2 directions (horizontal, vertical)
        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _zMovementInput;

        // Calculate the final moment velocity vector, by merging the horizontal and vertical vectors and adding the speed (since normalized vector has no speed but direction)
        Vector3 _movementVeloctyVector = (_movementHorizontal + _movementVertical).normalized * speed;

        // Appy Movement calculated above
        Move(_movementVeloctyVector);

        // Make the top tilt according to the joystick input
        // eventhough this is related with Physics, it's recommeded to put it in 'Update()' to show smoother tilting
        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -joystick.Horizontal * speed * tiltAmount);

    }

    #region Methods
    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;        
    }

    private void FixedUpdate()                                                   // To implement the movement on the object this script is attached to, Rigidbody is required
    {                                                                               // For Rigidbody, or everything for physics, 'FixedUpdate()' is recommended 
        if(velocityVector != Vector3.zero)                                    // since 'Update()' works every frame, which can vary depending on the performace
        {
            // Get rigidbody's current velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);    // 'velocityVector' = velocity created by the joystick input, 'velocity' = current movement

            // Apply a force by the amount of velocity chage to reach the target velocity
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, +maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.x, -maxVelocityChange, +maxVelocityChange);
            velocityChange.y = 0f; // since the top won't jump!

            rb.AddForce(velocityChange, ForceMode.Acceleration);
        }


    }

    #endregion
}