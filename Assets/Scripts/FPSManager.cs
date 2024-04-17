using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    // Set a target framerate of 120 fps
    // Disable vSync
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
}
