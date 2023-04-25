using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarAI : MonoBehaviour
{
    // List of points that a car can travel along
    [SerializeField]
    private List<Vector3> path = null;

    // Set How close a car can be to point when it decelerates 
    [SerializeField]
    private float arriveDistance = 0.3f;

    // Set how close a car can be to point when it de-spawns
    [SerializeField]
    private float lastPointArriveDistance = 0.1f;

    // Check the rotation of car to the point of distance, keep rotating the cat until it is at 
    [SerializeField]
    private float turningAngleOffset = 5;

    // Index number of points on the list of points that a car can travel
    private int index = 0;

    // Determine if a car is currently in a collision
    private bool collisionStop = false;

    // determine if the car is currently stops
    private bool stop;
    public bool Stop
    {
      get { return stop || collisionStop;}
      set { stop = value; }
    }

    // Raycastobject on car prefab
    [SerializeField]
    private GameObject raycastStartingPoint = null;

    // Length of raycast
    [SerializeField]
    private float collisionRaycastLength = 0.1f;

    [SerializeField]
    private Vector3 currentTargetPosition;
    // Unity event that sets car direction and speed in unity editor
    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    // Check if the car is at the last index in a path
    internal bool IsThisLastPathIndex()
    {
        return index >= path.Count-1;
    }

    // Drive a car until the index decrements down to 0 and then stop the car and de-spawn it
    private void Start()
    {
        if(path == null || path.Count == 0)
        {
            Stop = true;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    // Set the path of a car and de-spawn it at the terminus of the path. Decrement the index of path as the car travels along the path
    public void SetPath(List<Vector3> path)
    {
        // De-spawn car once it reaches the end of the path
        if(path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        // Find the next point of the path, increment the index as the car reaches the next point
        Vector3 relativePoint = transform.InverseTransformPoint(this.path[index + 1]);

        // Calculate the turning angle of the car based on the position of relativePoint
        float angle = Mathf.Atan2(relativePoint.x, relativePoint.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;
    }

    private void Update()
    {
        CheckIfArrived();
        Drive();
        CheckForCollisions();
    }

    // Use the raycast object to check if a car will collide with another
    private void CheckForCollisions()
    {
        // Use a layermask and bitshifting to check for other car objects on the car layer, if the raycast collides with a ray the car will stop
        if(Physics.Raycast(raycastStartingPoint.transform.position, transform.forward,collisionRaycastLength, 1 << gameObject.layer))
        {
            collisionStop = true;
        }
        else
        {
            collisionStop = false;
        }
        
    }

    private void Drive()
    {
        if (Stop)
        {
            OnDrive?.Invoke(Vector2.zero);
        }
        else
        {
            // Control the turning angle of a car
            Vector3 relativepoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;
            var rotateCar = 0;
            if(angle > turningAngleOffset)
            {
                rotateCar = 1;
            }else if(angle < -turningAngleOffset)
            {
                rotateCar = -1;
            }
            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    // Check if car has arrived at the end of its path
    private void CheckIfArrived()
    {
        if(Stop == false)
        {
            var distanceToCheck = arriveDistance;
            if(index == path.Count - 1)
            {
                distanceToCheck = lastPointArriveDistance;
            }
            if(Vector3.Distance(currentTargetPosition,transform.position) < distanceToCheck)
            {
                SetNextTargetIndex();
            }
        }
    }

    // Set the next target in the path
    private void SetNextTargetIndex()
    {
        index++;
        if(index >= path.Count)
        {
            Stop = true;
            Destroy(gameObject);
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }
}
