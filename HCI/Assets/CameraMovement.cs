using UnityEngine;
using UnityEngine.InputSystem;


public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 200f;   // Speed of camera movement
    public float mouseSensitivity = 1f; // Mouse sensitivity

    private float _movementX;
    private float _movementY;

    private float _rotateX;
    private float _rotateY;
    private Rigidbody _rigidbody;
    
    private bool isCollidingWithWall = false;
    

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // Stop movement when colliding with a wall
            isCollidingWithWall = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // Resume movement when no longer colliding with a wall
            isCollidingWithWall = false;
        }
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

        Vector3 movement = (forward * _movementY + right * _movementX) * (moveSpeed);

        // Apply movement to the object's position using transform
        _rigidbody.AddForce(movement, ForceMode.Force);

        Vector3 rotation = new Vector3(-_rotateY, _rotateX, 0.0f) * (mouseSensitivity);

        // Apply rotational force to Rigidbody using torque
        _rigidbody.AddTorque(rotation, ForceMode.Impulse);
    }
}
