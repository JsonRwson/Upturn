using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is applied to each layer of the background individually
// It applies a parallax effect to the background
public class ParallaxBG : MonoBehaviour
{
    // Store the length and start position of the background
    private float length, startPos;
    // Reference the main camera for the scene
    public GameObject mainCamera;
    // The parallex factor determines the speed of the parallex effect
    // It affects how fast the background layers move comapred to each other
    public float parallexFactor;
    // Start is called before the first frame update
    void Start()
    {
        // Calculate the length and start position for the background
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the reset threshold and distance from the start
        // The reset threshold decreases as the parallex factor increases
        float temp = mainCamera.transform.position.x * (1 - parallexFactor);
        // The distance from the start increases as the parallex factor increases
        float distFromStart = mainCamera.transform.position.x * parallexFactor;

        // Update the position of the background layer, using the start position and distance from the start
        // The distance from the start indicates how far the background layer should move
        // The temp variable represents the position of the camera, adjusted negatively by the parallax factor
        transform.position = new Vector2(startPos + distFromStart, transform.position.y);

        // If temp is greater than the starting position
        // The camera has moved to the right far enough that the start of the next cycle of the background layer should be visible
        if(temp > startPos)
        {
            // Increment the start position by the length of the background layer
            startPos += length;
        }
        // If temp is less than the starting position
        // The camera has moved to the left far enough that the end of the previous cycle of the background layer should be visible
        else if(temp < startPos)
        {
            // Decrement the start position by the legnth of the background layer
            startPos -= length;
        }
    }
}