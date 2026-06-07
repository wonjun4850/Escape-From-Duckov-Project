using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayResolution : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    #endregion

    #region 내부 변수
    private readonly List<(int w, int h)> _resolutions = new List<(int w, int h)>() { (1920, 1080), (1600, 900), (1280, 720) };
    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_resolutionDropdown == null)
        {
            return;
        }

        int savedIdx = PlayerPrefs.GetInt("ResolutionIndex", -1);

        if (savedIdx != -1 && savedIdx < _resolutions.Count)
        {
            _resolutionDropdown.value = savedIdx;
            ApplyResolution(savedIdx);
        }

        else
        {
            int currentWidth = Screen.width;
            int currentHeight = Screen.height;

            for (int i = 0; i < _resolutions.Count; i++)
            {
                if (_resolutions[i].w == currentWidth && _resolutions[i].h == currentHeight)
                {
                    _resolutionDropdown.value = i;
                    break;
                }
            }
        }
    }

    public void ChangeResolution(int idx)
    {
        ApplyResolution(idx);

        PlayerPrefs.SetInt("ResolutionIndex", idx);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int idx)
    {
        if (idx < 0 || idx >= _resolutions.Count) return;

        int width = _resolutions[idx].w;
        int height = _resolutions[idx].h;

        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }
}