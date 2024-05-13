using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    
    public float moveSpeed = 200f;   // Speed of camera movement
    public float mouseSensitivity = 1f; // Mouse sensitivity

    private Vector2 _movement;

    private float _rotateX;
    private float _rotateY;
    
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
    
        // Get the forward direction based on the character's rotation
        Vector3 forward = transform.forward;
        forward.y = 0f; // Ensure no vertical movement

        // Normalize the movement vector
        _movement = movementVector;
    }

    
    void OnLook(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _rotateX = movementVector.x; 
        _rotateY = movementVector.y;
        // Calculate rotation amount based on input
        float rotationAmount = _rotateX * 200 * Time.fixedDeltaTime;
        // Rotate the player around the y-axis
        transform.Rotate(Vector3.up, rotationAmount);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void HandleMovement(Vector2 inputVector) {
        // Get the player's forward direction
        Vector3 playerForward = transform.forward;
        playerForward.y = 0f; // Ensure no vertical movement

        // Get the player's right direction
        Vector3 playerRight = transform.right;
        playerRight.y = 0f; // Ensure no vertical movement

        // Normalize the player directions
        playerForward.Normalize();
        playerRight.Normalize();

        // Calculate the movement direction relative to the player's orientation
        Vector3 moveDir = playerForward * inputVector.y + playerRight * inputVector.x;
        moveDir.Normalize();

        // Calculate the movement distance
        float moveDistance = moveSpeed * Time.deltaTime;

        // Check if movement is possible
        float playerRadius = 2.5f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // If movement is not possible in the desired direction, try to move only along X or Z axis
        if (!canMove) {
            Vector3 moveDirX = playerRight * inputVector.x;
            moveDirX.Normalize();
            canMove = (inputVector.x < -0.5f || inputVector.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = playerForward * inputVector.y;
                moveDirZ.Normalize();
                canMove = (inputVector.y < -0.5f || inputVector.y > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        // If movement is possible, apply it relative to player's orientation
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }
    }

    
    
    

    // Update is called once per frame
    void Update()
    {
        
        // Check if the character can move
        float playerRadius = 0.7f;
        float playerHeight = 0.3f;
        
        HandleMovement(_movement);

        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDir, playerRadius))
        {
            // If the raycast hits something, draw a ray from the origin to the hit point
            Debug.Log("Drawing success ray");
            Debug.DrawRay(transform.position, moveDir, Color.red);
        }
        else
        {
            // If the raycast doesn't hit anything, draw a ray to the maximum distance
            Debug.Log("Drawing fail ray");
            Debug.DrawRay(transform.position, moveDir, Color.red);
        }

        // If cannot move, reset movement vector
        if (true)
        {
            Vector3 movementVector3 = _movement*moveSpeed;
            transform.position += movementVector3;
        }
        */
    }
}
