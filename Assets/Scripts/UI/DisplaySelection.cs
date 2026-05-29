using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaySelection : MonoBehaviour
{
    public void ChangeDisplayMode(int idx)
    {
        switch (idx)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;                
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;                
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;                
                break;
        }
    }
}