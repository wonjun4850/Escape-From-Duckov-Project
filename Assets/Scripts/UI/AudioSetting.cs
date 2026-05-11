using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    #region 인스펙터
    [Header("오디오 믹서")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("슬라이더 연결")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("텍스트 (숫자) 연결")]
    [SerializeField] private TextMeshProUGUI _masterText;
    [SerializeField] private TextMeshProUGUI _bgmText;
    [SerializeField] private TextMeshProUGUI _sfxText;
    #endregion

    private void Awake()
    {
        if (_audioMixer == null || _masterSlider == null || _bgmSlider == null || _sfxSlider == null || _masterText == null || _bgmText == null || _sfxText == null)
        {
            Debug.LogError("AudioSetting : 참조 실패 인스펙터 확인");
            return;
        }
    }

    private void Start()
    {
        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float vol = _masterSlider.value;
        _audioMixer.SetFloat("MasterVol", Mathf.Log10(Mathf.Max(0.0001f, vol / 100f)) * 20);
        _masterText.text = vol.ToString();
    }

    public void SetBGMVolume()
    {
        float vol = _bgmSlider.value;
        _audioMixer.SetFloat("BGMVol", Mathf.Log10(Mathf.Max(0.0001f, vol / 100f)) * 20);
        _bgmText.text = vol.ToString();
    }

    public void SetSFXVolume()
    {
        float vol = _sfxSlider.value;
        _audioMixer.SetFloat("SFXVol", Mathf.Log10(Mathf.Max(0.0001f, vol / 100f)) * 20);
        _sfxText.text = vol.ToString();
    }
}
