using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces the object to follow the player horizontally only
public class FollowPlayerHoirzontally : MonoBehaviour
{
    // Store an offset and reference to the player game object
    public GameObject player;
    public float xOffset = 0;
    public float yOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Only follow player horizontally
        // Store a temporary position
        var position = transform.position;
        // Match this position to the player's plus the offset
        position.x = player.transform.position.x + xOffset;
        // Apply this position to the object
        transform.position = position;
    }
}
