using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object flies across the screen
    private float screenWidth; // Width of the screen in world units

    void Start()
    {
        // Get the width of the screen in world units
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float cameraHeight = 2f * mainCamera.orthographicSize;
            screenWidth = cameraHeight * mainCamera.aspect;
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void Update()
    {
        // Move the object to the right
        transform.position += Vector3.left * speed * Time.deltaTime;

        //Kill the player
    }
}

