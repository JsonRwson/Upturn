using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsAnimations : MonoBehaviour
{
    public GameObject mainMenu;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the animator for the levels menu
        animator = GetComponent<Animator>();
    }

    // This is called when the slide out animation is finished
    // Set the levels menu to inactive
    // Update the animation state
    void SetLevelsInactive()
    {
        gameObject.SetActive(false);
        animator.SetInteger("InLevels", 0);
    }

    // This is called halfway through the slide out animation
    // So that the main menu re-appears smoothly
    void SetMainMenuActive()
    {
        mainMenu.SetActive(true);
    }
}
