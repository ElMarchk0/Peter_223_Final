using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatically attach a rigidbody
public class CarController : MonoBehaviour
{
    Rigidbody rb;

    // Speed of car
    [SerializeField]
    private float power = 5;

    // Agility of car
    [SerializeField]
    private float torque = 0.5f;

    [SerializeField]
    private float maxSpeed = 5;

    // Value assigned to AI script to move it in the right direction
    [SerializeField]
    private Vector2 movementVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Move method that is called in CarAI
    public void Move(Vector2 movementInput)
    {
        this.movementVector = movementInput;
    }

    private void FixedUpdate()
    {
        // If the speed of the car is less than max speed, accelerate it.
        if(rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movementVector.y * transform.forward * power);
        }
        // Make sure the car can turn
        rb.AddTorque(movementVector.x * Vector3.up * torque * movementVector.y);
    }
}
