using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the UI for the main menu
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pressing starts takes the player into endless
    // Disable the ui canvas for the main menu
    public void StartEndless()
    {
        gameObject.SetActive(false);
    }

    // Close the game when clicking quit
    public void QuitGame()
    {
        Application.Quit();
    }
}
