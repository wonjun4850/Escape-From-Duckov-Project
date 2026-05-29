using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum EGameState
    {
        Playing,
        Escape,
        Dead
    }

    #region 인스펙터
    private Player _player;
    #endregion

    #region 내부 변수
    public static GameManager Instance { get; private set; }
    private EGameState _currentState = EGameState.Playing;

    private int _killExp = 0;
    private int _farmingExp = 0;
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }
    }

    private void SavePlayerData()
    {
        var data = DataManager.Instance;

        data.CurrentHp = _player.Hp.GetCurrentHp();
        data.CurrentEnergy = _player.Survival.GetCurrentEnergy();
        data.CurrentHydration = _player.Survival.GetCurrentHydration();
    }

    #region 외부 호출 함수
    public void MoveNextScene(string sceneID, string actionMap, bool clickDirection = false)
    {
        if (_currentState != EGameState.Playing)
        {
            return;
        }

        SavePlayerData();

        SceneLoader.Instance.LoadScene(sceneID, actionMap, clickDirection);
    }

    public void OnPlayerEscape()
    {
        if (_currentState != EGameState.Playing)
        {
            return;
        }

        _currentState = EGameState.Escape;

        SavePlayerData();

        SoundManager.Instance.PlaySFX("Escape");
        SceneLoader.Instance.ShowResultUI(_killExp + _farmingExp, true);
    }

    public void OnPlayerDead()
    {
        if (_currentState != EGameState.Playing)
        {
            return;
        }

        _currentState = EGameState.Dead;

        var data = DataManager.Instance;

        data.CurrentHp = data.MaxHp;
        data.CurrentEnergy = data.MaxEnergy;
        data.CurrentHydration = data.MaxHydration;

        SoundManager.Instance.PlaySFX("Player_Dead");
        SceneLoader.Instance.ShowResultUI(_killExp, false);
    }

    public void FinishResultEscapeAndReturnToBase()
    {
        _killExp = 0;
        _farmingExp = 0;
        SceneLoader.Instance.LoadScene("Base1", "Ingame");
    }

    public void FinishResultDeadAndReturnToBase()
    {
        _killExp = 0;
        _farmingExp = 0;
        SceneLoader.Instance.LoadScene("Base1", "Ingame", true);
    }

    public void AddKillExp(int Amount)
    {
        _killExp += Amount;
    }

    public void AddFarmingExp(int Amount)
    {
        _farmingExp += Amount;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public EGameState GetState()
    {
        return _currentState;
    }

    public void SetState(EGameState state)
    {
        _currentState = state;
    }
    #endregion
}