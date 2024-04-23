using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Controls the UI for the main menu
public class MainMenu : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject optionsPanel;
    public GameObject levelsPanel;
    public GameObject GameOverlay;

    public GameObject EnvironmentSpawner;
    
    private Animator levelsAnimator;
    private Animator optionsAnimator;

    public AudioMixer audioMixer;
    public float previousVolume;
    public float mutedVolume = -80;

    // Start is called before the first frame update
    void Start()
    {
        // Get the animator references, set the default states
        optionsAnimator = optionsPanel.GetComponent<Animator>();
        optionsAnimator.SetInteger("InOptions", 0);
        optionsPanel.SetActive(false);
        
        levelsAnimator = levelsPanel.GetComponent<Animator>();
        levelsAnimator.SetInteger("InLevels", 0);
        levelsPanel.SetActive(false);

        GameOverlay.SetActive(false);

        // Store the starting volume
        audioMixer.GetFloat("MasterVolume", out previousVolume);
    }

    // Pressing starts takes the player into endless
    // Disable the ui canvas for the main menu
    public void StartEndless()
    {
        EnvironmentSpawner.GetComponent<EnvSpawner>().StartEndless();
        GameOverlay.SetActive(true);
        gameObject.SetActive(false);
    }

    // Close the game when clicking quit
    public void QuitGame()
    {
        Application.Quit();
    }

    // Hide the start menu, show the options menu
    // Start the slide in animation for the options menu
    public void ShowOptions()
    {
        startPanel.SetActive(false);
        optionsPanel.SetActive(true);
        optionsAnimator.SetInteger("InOptions", 1);
    }

    // Hide the start menu, show the options menu
    // Start the slide in animation for the options menu
    public void ShowLevels()
    {
        startPanel.SetActive(false);
        levelsPanel.SetActive(true);
        levelsAnimator.SetInteger("InLevels", 1);
    }

    // Restore the start menu and slide out the options menu
    public void ReturnMenuAnimtation()
    {
        optionsAnimator.SetInteger("InOptions", 2);
        levelsAnimator.SetInteger("InLevels", 2);
    }

    // Toggle mute for master volume
    // The previously used volume is stored to restore when unmuting
    public void ToggleSound()
    {
        // Get the current volume
        float Volume;
        bool Result = audioMixer.GetFloat("MasterVolume", out Volume);
        
        // If the volume is not muted
        if(Volume > mutedVolume)
        {
            // Store the current volume to restore later
            previousVolume = Volume;
            // And mute the sound
            audioMixer.SetFloat("MasterVolume", mutedVolume);
        }
        else // If the volume is muted
        {
            // Restore the previously used volume
            audioMixer.SetFloat("MasterVolume", previousVolume);
        }
    }
}
