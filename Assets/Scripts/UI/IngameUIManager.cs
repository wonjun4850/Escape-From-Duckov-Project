using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    #region 인스펙터
    [Header("Canvas 연결")]
    [SerializeField] private GameObject _canvasHUD_Camera;
    [SerializeField] private GameObject _canvasHUD_OverRay;
    [SerializeField] private GameObject _canvasInteraction;
    [SerializeField] private GameObject _canvasPopup;

    [Header("Panel 연결")]
    // 상호작용
    [SerializeField] private GameObject _interactionPanel;

    // 팝업
    [SerializeField] private GameObject _PausePanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _storagePanel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _QuestPanel;
    #endregion

    #region 내부 변수
    public static IngameUIManager Instance { get; private set; }
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

    void Start()
    {
        _canvasHUD_Camera.SetActive(true);
        _canvasHUD_OverRay.SetActive(true);
        _canvasInteraction.SetActive(false);
        _canvasPopup.SetActive(false);

        InputCommandHub.Instance.RegisterCommands("Player.Pause", new SimpleCommand(OnPause));
        InputCommandHub.Instance.RegisterCommands("Player.Inventory", new SimpleCommand(OnInventory));
        InputCommandHub.Instance.RegisterCommands("Player.Interact", new SimpleCommand(OnInteract));
        InputCommandHub.Instance.RegisterCommands("Player.Map", new SimpleCommand(OnMap));
        InputCommandHub.Instance.RegisterCommands("Player.Quest", new SimpleCommand(OnQuest));        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnPause()
    {
        Debug.Log("일시정지");
    }

    private void OnInventory()
    {
        Debug.Log("인벤토리");
    }

    private void OnInteract()
    {
        Debug.Log("상호작용");
    }

    private void OnMap()
    {
        Debug.Log("맵");
    }

    private void OnQuest()
    {
        Debug.Log("퀘스트");
    }

    #region 외부 호출 함수
    public void BindPlayerUI(Player player)
    {
        if (player == null)
        {
            Debug.LogError("player = null 확인 필요");
            return;
        }

        Canvas canvasHUD = _canvasHUD_Camera.GetComponent<Canvas>();

        if (canvasHUD != null)
        {
            canvasHUD.worldCamera = Camera.main;
        }

        GetComponentInChildren<UI_HpGauge>(true)?.Setup(player.Hp);
        GetComponentInChildren<UI_StaminaGauge>(true)?.SetUp(player.Stamina, player.transform);
        GetComponentInChildren<UI_Range>(true)?.SetUp(player.transform);
        UI_SurvivalGauge[] survival = GetComponentsInChildren<UI_SurvivalGauge>(true);
        foreach (var s in survival)
        {
            s.SetUp(player.Survival);
        }
    }
    #endregion
}