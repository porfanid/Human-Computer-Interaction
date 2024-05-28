using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class NewBehaviourScript : MonoBehaviour
{
    
    public float moveSpeed = 200f;   // Speed of camera movement
    public float mouseSensitivity = 1f; // Mouse sensitivity
    
    public Image c;

    public Button button;
    public TMP_Text info;
    public Camera camera;

    private Vector2 _movement;

    private float _rotateX;
    private float _rotateY;

    private Dictionary<string, string> intel;
    
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        // Get the forward direction based on the character's rotation
        Vector3 forward = transform.forward;
        forward.y = 0f; // Ensure no vertical movement

        // Normalize the movement vector
        _movement = movementVector;
    }


    private void OnFire()
    {
        Debug.Log("On fire has been called");
        RaycastHit[] hits;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = camera.ScreenPointToRay(screenCenter);

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

                try
                {
                    // Set the information text from the dictionary
                    info.SetText(intel[nearestObject.name]);
                    c.gameObject.SetActive(true);
                    button.gameObject.SetActive(true);
                    info.gameObject.SetActive(true);
                    Debug.Log("Nearest object: " + nearestObject.name);
                }
                catch (KeyNotFoundException)
                {
                    Debug.Log(nearestObject.name);
                }
            }
        }
    }


    
    void OnLook(InputValue movementValue)
    {
        if (!info.gameObject.activeSelf)
        {
            return;
        }
        Vector2 movementVector = movementValue.Get<Vector2>();
        _rotateX = movementVector.x; 
        _rotateY = movementVector.y;
        // Calculate rotation amount based on input
        float rotationAmount = _rotateX * mouseSensitivity * Time.fixedDeltaTime;
        // Rotate the player around the y-axis
        transform.Rotate(Vector3.up, rotationAmount);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        intel = new Dictionary<string, string>();
        // Determine the file path
        string filePath = Path.Combine(Application.dataPath, "ekth.json");

        // Read the JSON file
        string jsonString = File.ReadAllText(filePath);
        Debug.Log("JSON Content: " + jsonString);

        // Parse the JSON data
        ItemCollection itemCollection = JsonUtility.FromJson<ItemCollection>(jsonString);

        // Convert to dictionary
        Dictionary<string, Item> itemDict = itemCollection.ToDictionary();
        Debug.Log("Read the data");

        // Print the parsed data
        foreach (var item in itemDict)
        {
            intel[item.Key] = item.Value.title + "\n" + item.Value.info;
        }
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
        float playerRadius = 5f;
        float playerHeight = 4f;
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
    }
    
    [System.Serializable]
    public class KeyValuePair
    {
        public string key;
        public Item value;
    }
    
    [System.Serializable]
    public class ItemCollection
    {
        public List<KeyValuePair> items;

        public Dictionary<string, Item> ToDictionary()
        {
            Dictionary<string, Item> dict = new Dictionary<string, Item>();
            foreach (var kvp in items)
            {
                dict[kvp.key] = kvp.value;
            }
            return dict;
        }
    }
}
