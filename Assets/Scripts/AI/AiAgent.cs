﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCity.AI
{
    [RequireComponent(typeof(Animator))]
    public class AiAgent : MonoBehaviour
    {
        public event Action OnDeath;

        Animator animator;
        public float speed = 0.2f;
        public float rotationSpeed = 10f;

        List<Vector3> pathToGo = new List<Vector3>();
        bool moveFlag = false;
        int index = 0;
        Vector3 endPosition;

        // Initialize an ai agent, and assign a path to it
        public void Initialize(List<Vector3> path)
        {
            pathToGo = path;
            index = 1;
            moveFlag = true;
            endPosition = pathToGo[index];
            animator = GetComponent<Animator>();
            animator.SetTrigger("Walk");
        }

        private void Update()
        {
            if (moveFlag)
            {
                PerformMovement();
            }
        }

        // Move the agent from one point to another
        private void PerformMovement()
        {
            if(pathToGo.Count> index)
            {
                float distanceToGo = MoveTheAgent();
                if(distanceToGo < 0.05f)
                {
                    index++;
                    if(index >= pathToGo.Count)
                    {
                        moveFlag = false;
                        Destroy(gameObject);
                        return;
                    }
                    endPosition = pathToGo[index];
                }
            }
        }

        // Set the distance for an agent to move
        private float MoveTheAgent()
        {
            float step = speed * Time.deltaTime;
            // Use the x value to move the agent the they travel at the correct height
            Vector3 endPositionCorrect = new Vector3(endPosition.x, transform.position.y, endPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, endPositionCorrect, step);

            var lookDirection = endPositionCorrect - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotationSpeed);
            return Vector3.Distance(transform.position, endPositionCorrect);
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}

