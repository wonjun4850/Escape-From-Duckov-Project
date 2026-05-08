using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct SoundData
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0f, 1f)] private float _volume;
        [SerializeField] private string _description;

        public string Name => _name;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public string Description => _description;
    }

    #region 인스펙터
    [Header("오디오소스 연결")]
    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    [Header("배경음 목록")]
    [SerializeField] private List<SoundData> _bgmList = new List<SoundData>();

    [Header("효과음 목록")]
    [SerializeField] private List<SoundData> _sfxList = new List<SoundData>();
    #endregion

    #region 내부 변수
    public static SoundManager Instance { get; private set; }

    private readonly Dictionary<string, SoundData> _bgmDict = new Dictionary<string, SoundData>();
    private readonly Dictionary<string, SoundData> _sfxDict = new Dictionary<string, SoundData>();
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _bgmAudioSource.loop = true;
        _sfxAudioSource.loop = false;
        
        foreach (var bgm in _bgmList)
        {
            if (!_bgmDict.ContainsKey(bgm.Name))
            {
                _bgmDict.Add(bgm.Name, bgm);
            }
        }

        foreach (var sfx in _sfxList)
        {
            if (!_sfxDict.ContainsKey(sfx.Name))
            {
                _sfxDict.Add(sfx.Name, sfx);
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private IEnumerator CoFadeOutBGM(float duration)
    {
        float startValue = _bgmAudioSource.volume;

        while (_bgmAudioSource.volume > 0)
        {
            _bgmAudioSource.volume -= startValue * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        _bgmAudioSource.Stop();
        _bgmAudioSource.volume = startValue;
    }

    private IEnumerator CoFadeInBGM(float targetVolume, float duration)
    {
        _bgmAudioSource.volume = 0;

        while (_bgmAudioSource.volume < targetVolume)
        {
            _bgmAudioSource.volume += targetVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        _bgmAudioSource.volume = targetVolume;
    }

    #region 외부 호출 함수
    public void PlayBGM(string bgmName, float fadeDuration = 0f)
    {
        if (_bgmDict.TryGetValue(bgmName, out SoundData data))
        {
            _bgmAudioSource.clip = data.Clip;
            _bgmAudioSource.Play();

            if (fadeDuration > 0)
            {
                StartCoroutine(CoFadeInBGM(data.Volume, fadeDuration));
            }
            else
            {
                _bgmAudioSource.volume = data.Volume;
            }
        }

        else
        {
            Debug.Log($"SoundManager: {bgmName} 사운드 찾지 못함");
        }
    }

    public void FadeOutBGM(float duration)
    {
        StartCoroutine(CoFadeOutBGM(duration));
    }

    public void PlaySFX(string sfxName)
    {
        if (_sfxDict.TryGetValue(sfxName, out SoundData data))
        {
            _sfxAudioSource.PlayOneShot(data.Clip, data.Volume);
        }

        else
        {
            Debug.Log($"SoundManager: {sfxName} 사운드 찾지 못함");
        }
    }

    public void PlayObjectSFX(AudioSource ob, string sfxName, float minDistance = 1f, float maxDistance = 50)
    {
        if (_sfxDict.TryGetValue(sfxName, out SoundData data))
        {
            ob.clip = data.Clip;
            ob.volume = data.Volume;

            ob.spatialBlend = 1.0f;
            ob.rolloffMode = AudioRolloffMode.Linear;

            ob.minDistance = minDistance;
            ob.maxDistance = maxDistance;

            ob.Play();
        }

        else
        {
            Debug.Log($"SoundManager: {sfxName} 사운드 찾지 못함");
        }
    }
    #endregion
}