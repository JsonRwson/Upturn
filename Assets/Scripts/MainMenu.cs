using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the UI for the main menu
public class MainMenu : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject OptionsPanel;
    private Animator OptionsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        OptionsAnimator = OptionsPanel.GetComponent<Animator>();
        OptionsAnimator.SetInteger("InOptions", 0);
        OptionsPanel.SetActive(false);
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

    public void ShowOptions()
    {
        StartPanel.SetActive(false);
        OptionsPanel.SetActive(true);
        OptionsAnimator.SetInteger("InOptions", 1);
    }

    public void ReturnMenuAnimtation()
    {
        OptionsAnimator.SetInteger("InOptions", 2);
    }
}
