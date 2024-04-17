using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBoundsChecker : MonoBehaviour
{
    // When the player collides with a trigger collider
    void OnTriggerEnter2D(Collider2D collision)
    {
        // If they hit the upper or lower out of bounds colliders
        if(collision.gameObject.CompareTag("OutOfBounds"))
        {
            // Restart the scene
            // Todo proper game over stuff
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
