using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

// Applied to the player, determines the player controls and movement, utilizing with the character controller
// The player constantly moves, touch inputs flip the gravity
public class PlayerMovement : MonoBehaviour
{
    // Reference the character controller to move the player
    public CharacterController2D controller;
    // Reference the animator  to change the player animation states
    public Animator animator;

    private Vector2 pointerPosition;
    private Vector2 startPosition;
    private GhostTrail ghost;

    private float playerSpeed;
    // Can be modified from within the editor
    public float maxSpeed = 1.5f;
    public float startSpeed = 0.3f;
    public float timeToMaxSpeed = 100;
    public float speedInMenu = 0.5f;
    public float gravityScaleLower = 3f;
    public float gravityScaleUpper = 8f;

    private float currentGravityScale;
    private float startTime;
    private bool isInMenu = true;


    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = startSpeed;
        startPosition = transform.position;
        ghost = GetComponent<GhostTrail>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in the menu, the character will run in the background at a constant speed
        // Otherwise, we want to increment the speed overtime
        if(!isInMenu)
        {
            // Calculate the elapsed time since the start of the game
            float elapsedTime = Time.time - startTime;

            // Use a sigmoid function to calculate the player's speed
            // A start speed is set, which increases overtime to level at the max speed
            playerSpeed = (maxSpeed - startSpeed) / (1 + Mathf.Exp(-elapsedTime / timeToMaxSpeed + 5)) + startSpeed;
            
            float speedRatio = (playerSpeed - startSpeed) / (maxSpeed - startSpeed);
            controller.gravityScale = gravityScaleLower + speedRatio * (gravityScaleUpper - gravityScaleLower);
            
            // Calculate the new gravity scale
            float newGravityScale = playerSpeed * controller.gravityScale + 3;

            // Clamp the new gravity scale to be within the range [3, 8]
            controller.gravityScale = Mathf.Clamp(newGravityScale, gravityScaleLower, gravityScaleUpper);

            // Constantly move the player forward at the current speed
            controller.Move(playerSpeed, false, false);
            // Set the animation parameter to play the run animation
            animator.SetFloat("Speed", 1);

            // Take touch input only once per frame
            if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                // Store the position of the tap
                pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                // Debug.Log("Touch detected at X position: " + pointerPosition.x);

                // If the player is grounded currently
                if(controller.getIsGrounded())
                {
                    // Switch the gravity using the controller
                    // Update the animation parameter to play the jump animation
                    ghost.generateGhosts = true;
                    animator.SetBool("IsJumping", true);
                    controller.SwitchGravity();
                }
            }
        }
        else
        {
            // If the game is in menu mode, move the player at the start speed
            controller.Move(0.5f, false, false);
            animator.SetFloat("Speed", 1);
        }
    }

    public Vector2 GetStartPosition()
    {
        return startPosition;
    }

    // Upon starting endless mode, initialize the start time
    public void StartEndless()
    {
        isInMenu = false;
        startTime = Time.time;
    }

    // When the character lands, update the animation parameter
    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        ghost.generateGhosts = false;
    }

}
