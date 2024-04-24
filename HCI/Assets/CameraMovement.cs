using UnityEngine;
using UnityEngine.InputSystem;


public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 200f;   // Speed of camera movement
    public float mouseSensitivity = 10f; // Mouse sensitivity

    private float _movementX;
    private float _movementY;

    private float _rotateX;
    private float _rotateY;
    private Rigidbody _rigidbody;
    

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }
    
    
    
    

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _movementX = movementVector.x; 
        _movementY = movementVector.y; 
    }

    void OnFire()
    {
            RaycastHit[] hits;
            Camera c = GetComponent<Camera>();
            Ray ray = c.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast and get all hits
            hits = Physics.RaycastAll(ray);

            if (hits.Length > 0)
            {
                // Initialize variables to track nearest object
                GameObject nearestObject = null;
                float nearestDistance = Mathf.Infinity;

                // Loop through all hits to find the nearest valid object
                foreach (var hit in hits)
                {
                    GameObject hitObject = hit.collider.gameObject;

                    // Check if the hit object is closer than the current nearest object
                    float distanceToObject = Vector3.Distance(transform.position, hitObject.transform.position);

                    if (distanceToObject < nearestDistance)
                    {
                        nearestObject = hitObject;
                        nearestDistance = distanceToObject;
                    }
                }
                // Check if a nearest object was found
                if (nearestObject != null)
                {
                    Debug.Log("Nearest object: " + nearestObject.name);

                    // Perform actions with the nearest object (e.g., highlight, interact)
                    // Example: nearestObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
    }

    void OnLook (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _rotateX = movementVector.x; 
        _rotateY = movementVector.y;
    }

    private void FixedUpdate() 
    {
        Vector3 forward = transform.forward;
        forward.y = 0.0f; // Ignore vertical component for movement on the XZ plane
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0.0f; // Ignore vertical component for movement on the XZ plane
        right.Normalize();

        Vector3 movement = (forward * _movementY + right * _movementX) * (moveSpeed * Time.fixedDeltaTime);
        
        Vector3 targetPosition = transform.position + movement;

        // Clamp the target position within the desired limits
        targetPosition.x = Mathf.Clamp(targetPosition.x, -450f, 450f);
        targetPosition.z = Mathf.Clamp(targetPosition.z, -450f, 450f);

        // Apply the clamped movement to the object's position using transform
        transform.position = targetPosition;

        // Apply movement to the object's position using transform
        transform.Translate(movement, Space.World);

        Vector3 rotation = new Vector3(_rotateY, _rotateX, 0.0f) * (mouseSensitivity * Time.fixedDeltaTime);
        transform.eulerAngles-=rotation;
    }
}
