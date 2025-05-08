using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public Transform targetPosition;
    public Transform elevatorStart;
    public float speed = 2f;
    public float activateDelay = 0.5f;

    private Vector3 startPosition;
    private bool isMoving = false;
    public bool isGoingUp = true;
    private float timer = 0f;
    private Transform playerTransform;
    private Transform originalTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        originalTargetPosition = targetPosition;         
    }
    
    public void ActivateElevator(Transform player)
    {
        if(!isMoving)
        {
            isMoving = true;
            timer =0f;
            playerTransform = player;
            if(playerTransform != null)
            {
                playerTransform.SetParent(transform, true);
            }

            Debug.Log("Button Pressed. isGoingUp: " + isGoingUp);
            if(isGoingUp)
            {
                targetPosition = originalTargetPosition;
                Debug.Log("Target set to: " + targetPosition.name);
            }
            else
            {
                targetPosition = elevatorStart;
                Debug.Log("Target set to: " + targetPosition.name);
            }

             isGoingUp = !isGoingUp; //Toggle state for next press.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            timer += Time.deltaTime;
            if(timer >= activateDelay)
            {
                float t = Mathf.Clamp01((timer - activateDelay) * speed);
                Vector3 currentStartPos = isGoingUp ? startPosition : transform.position;
                Vector3 target = targetPosition.position;
                transform.position = Vector3.Lerp(startPosition, target, t);

                //Check if target is reached
                if(t >= 1f)
                {
                    isMoving = false;
                    if(playerTransform != null)
                    {
                        playerTransform.SetParent(null, true);
                        playerTransform = null;
                    }
                }
            }
        }
        
    }
}
