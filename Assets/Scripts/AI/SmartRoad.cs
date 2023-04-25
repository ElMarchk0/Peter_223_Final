using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartRoad : MonoBehaviour
{
    Queue<CarAI> trafficQueue = new Queue<CarAI>();

    // Access the current car that is going through the intersection
    public CarAI currentCar;

    [SerializeField]
    private bool pedestrianWaiting = false;


    [SerializeField]
    private bool pedestrianWalking = false;


    [SerializeField]
    public UnityEvent OnPedestrianCanWalk { get; set; }

    // Check for oncoming car, the trigger is placed in the middle on an intersection
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            // If the there is a car in the intersection, get the ai component
            var car = other.GetComponent<CarAI>();
            // If car is not null and if there is an other car and if the car is not at the terminus of its path, place it in the que and stop it
            if(car != null && car != currentCar && !car.IsThisLastPathIndex())
            {
                trafficQueue.Enqueue(car);
                car.Stop = true;
            }
        }
    }

    // Check for outgoing car, the trigger is placed in the middle on an intersection
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            var car = other.GetComponent<CarAI>();
            // Remove car from front of trafficQueue
            if(car != null)
            {
                RemoveCar(car);
            }
        }
    }

    // Set the currentCar to null
    private void RemoveCar(CarAI car)
    {
        if(car == currentCar)
        {
            currentCar = null;
        }
    }

    // Set the pedestrian flags
    public void SetPedestrianFlag(bool val)
    {
        if (val)
        {
            pedestrianWaiting = true;
        }
        else
        {
            pedestrianWaiting = false;
            pedestrianWalking = false;
        }
    }

    private void Update()
    {
        // if current car is equal to null, check for other cars in the traffic queue
        if(currentCar == null)
        {
            // Send the next car the queue through the intersection
            if(trafficQueue.Count > 0 && pedestrianWaiting == false && pedestrianWalking == false)
            {
                currentCar = trafficQueue.Dequeue();
                currentCar.Stop = false;
            }
            // Check for pedestrians 
            else if(pedestrianWalking || pedestrianWaiting)
            {
                OnPedestrianCanWalk?.Invoke();
                pedestrianWalking = true;
            }
        }
    }
}
