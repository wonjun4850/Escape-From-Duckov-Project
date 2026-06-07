using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    #region 인스펙터
    [Header("Canvas 연결")]
    [SerializeField] private Canvas _canvasHUD_Camera;
    [SerializeField] private Canvas _canvasHUD_OverRay;
    [SerializeField] private Canvas _canvasInteraction;
    [SerializeField] private Canvas _canvasPopup;

    [Header("Panel 연결")]
    [SerializeField] private UI_HUDTransition _hudTransition;
    [SerializeField] private UI_PausePanel _pausePanel;
    [SerializeField] private UI_InventoryTransition _inventoryTransition;
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _questPanel;

    [Header("탈출UI 연결")]
    [SerializeField] private UI_EscapeTimer _escapeTimer;
    #endregion

    #region 내부 변수
    public static IngameUIManager Instance { get; private set; }
    private IngameCamera _ingameCamera;
    private PlayerMovement _playerMovement;
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
        _canvasHUD_Camera.enabled = true;
        _canvasHUD_OverRay.enabled = true;
        _canvasInteraction.enabled = true;
        _canvasPopup.enabled = true;

        _pausePanel.gameObject.SetActive(false);
        _inventoryTransition.gameObject.SetActive(false);

        // Ingame 키 등록
        InputCommandHub.Instance.RegisterCommands("Player.Pause", new SimpleCommand(OnPause));
        InputCommandHub.Instance.RegisterCommands("Player.Inventory", new SimpleCommand(OnInventory));
        InputCommandHub.Instance.RegisterCommands("Player.Interact", new SimpleCommand(OnInteract));
        InputCommandHub.Instance.RegisterCommands("Player.Map", new SimpleCommand(OnMap));
        InputCommandHub.Instance.RegisterCommands("Player.Quest", new SimpleCommand(OnQuest));

        // Ui 키 등록
        InputCommandHub.Instance.RegisterCommands("Ui.PauseCancel", new SimpleCommand(OnMasterCancel));
        InputCommandHub.Instance.RegisterCommands("Ui.InventoryCancel", new SimpleCommand(OnInventoryCancel));
        InputCommandHub.Instance.RegisterCommands("Ui.InteractionCancel", new SimpleCommand(OnInteractionCancel));
        InputCommandHub.Instance.RegisterCommands("Ui.MapCancel", new SimpleCommand(OnMapCancel));
        InputCommandHub.Instance.RegisterCommands("Ui.QuestCancel", new SimpleCommand(OnQuestCancel));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
        {
            Destroy(gameObject);
            return;
        }

        if (_inventoryTransition != null)
        {
            bool isActive = (scene.name != "GroundZero");

            _inventoryTransition.SetStorageActiveByScene(isActive);
        }
    }

    #region Ingame 키맵
    private void OnPause()
    {
        _pausePanel.Show();
        _hudTransition.Hide();
        _ingameCamera.SetFocusPlayer(true);
        _playerMovement.SetRotateToMouse(false);
    }

    private void OnInventory()
    {
        _inventoryTransition.Show();
        _hudTransition.Hide();
        _ingameCamera.SetFocusPlayer(true);
        _playerMovement.SetRotateToMouse(false);
    }

    private void OnInteract()
    {
        //_hudTransition.Hide();
        //_ingameCamera.SetFocusPlayer(true);
        //_playerMovement.SetRotateToMouse(false);
        Debug.Log("상호작용");
    }

    private void OnMap()
    {
        //_hudTransition.Hide();
        //_ingameCamera.SetFocusPlayer(true);
        //_playerMovement.SetRotateToMouse(false);
        Debug.Log("맵");
    }

    private void OnQuest()
    {
        //_hudTransition.Hide();
        //_ingameCamera.SetFocusPlayer(true);
        //_playerMovement.SetRotateToMouse(false);
        Debug.Log("퀘스트");
    }
    #endregion

    #region Ui 키맵
    private void OnMasterCancel()
    {
        if (_pausePanel.gameObject.activeSelf)
        {
            _hudTransition.Show();
            _pausePanel.Hide();
            _ingameCamera.SetFocusPlayer(false);
            _playerMovement.SetRotateToMouse(true);
            return;
        }

        if (_inventoryTransition.gameObject.activeSelf)
        {
            _hudTransition.Show();
            _inventoryTransition.Hide();
            _ingameCamera.SetFocusPlayer(false);
            _playerMovement.SetRotateToMouse(true);
            return;
        }

        //if (_interactionPanel.activeSelf)
        //{
        //
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //    return;
        //}
        //
        //if (_mapPanel.activeSelf)
        //{
        //
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //    return;
        //}
        //
        //if (_questPanel.activeSelf)
        //{
        //
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //    return;
        //}

        //_pausePanel.Show();
    }

    private void OnInventoryCancel()
    {
        if (_inventoryTransition.gameObject.activeSelf)
        {
            _hudTransition.Show();
            _inventoryTransition.Hide();
            _ingameCamera.SetFocusPlayer(false);
            _playerMovement.SetRotateToMouse(true);
        }
    }

    private void OnInteractionCancel()
    {
        //if (_interactionPanel.activeSelf)
        //{
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //}
    }

    private void OnMapCancel()
    {
        //if (_mapPanel.activeSelf)
        //{
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //}
    }

    private void OnQuestCancel()
    {
        //if (_questPanel.activeSelf)
        //{
        //    _hudTransition.Show();
        //    _ingameCamera.SetFocusPlayer(false);
        //    _playerMovement.SetRotateToMouse(true);
        //}
    }
    #endregion

    #region 외부 호출 함수
    public void BindPlayerUI(Player player)
    {
        if (player == null)
        {
            Debug.LogError("player = null 확인 필요");
            return;
        }

        _playerMovement = player.Movement;

        if (_canvasHUD_Camera != null)
        {
            _canvasHUD_Camera.worldCamera = Camera.main;
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

    public void RegisterCamera(IngameCamera ingameCamera)
    {
        _ingameCamera = ingameCamera;
    }

    public void StartEscapeTimer(float escapeTime)
    {
        _escapeTimer?.StartUI(escapeTime);
    }

    public void StopEscapeTimer()
    {
        _escapeTimer?.EndUI();
    }

    public void UpdateEscapeTimer(float remainingTime)
    {
        _escapeTimer?.UpdateUI(remainingTime);
    }

    public void ClosePausePanel()
    {
        if (_pausePanel.gameObject.activeSelf)
        {
            _pausePanel.Hide();
            _hudTransition.Show();
            _ingameCamera.SetFocusPlayer(false);
        }
    }
    #endregion
}