using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parallax effect script applied to each layer of the background
// Each layer has its own parallax amount set within the editor
// Makes the background layers appear to move at different speeds
// Repeats the background layers infront and behind to create an infinite background
public class ParallaxEffect : MonoBehaviour
{
    private float startingPos; // This is starting position of the sprites
    private float lengthOfSprite;    // This is the length of the sprites
    public float AmountOfParallax;  // This is amount of parallax scroll
    // A value of 0 means the background doesn’t move at all (i.e., it’s very far away)
    // A value of 1 means it moves with the camera (i.e., it’s very close).

    public Camera MainCamera;   // Reference of the camera

    // Start is called before the first frame update
    void Start()
    {
        // Getting the starting X position of sprite.
        startingPos = transform.position.x;    
        // Getting the length of the sprites.
        lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = MainCamera.transform.position;
        // Determine the distance which the layer mustbe moved according to the camera
        float Temp = position.x * (1 - AmountOfParallax);
        float distance = position.x * AmountOfParallax;

        // Move the layer according to the parallax effect and camera position
        Vector3 NewPosition = new Vector3(startingPos + distance, transform.position.y, transform.position.z);
        transform.position = NewPosition;

        // If the camera has moved past half of the layer, move it forward
        if(Temp > startingPos + (lengthOfSprite / 2))
        {
            startingPos += lengthOfSprite;
        }
        // If it has moved backwards, half of the layer, moved it back
        else if(Temp < startingPos - (lengthOfSprite / 2))
        {
            startingPos -= lengthOfSprite;
        }
    }
}
