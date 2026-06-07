using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplaySelection : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ
    [SerializeField] private TMP_Dropdown _screenModeDropdown;
    #endregion

    void Start()
    {
        Init();
    }

    private void Init()
    {
        int savedIdx = PlayerPrefs.GetInt("DisplayModeIndex", 0);

        if (_screenModeDropdown != null)
        {
            _screenModeDropdown.value = savedIdx;
        }

        ApplyDisplayMode(savedIdx);
    }

    public void ChangeDisplayMode(int idx)
    {
        ApplyDisplayMode(idx);

        PlayerPrefs.SetInt("DisplayModeIndex", idx);
        PlayerPrefs.Save();
    }

    private void ApplyDisplayMode(int idx)
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