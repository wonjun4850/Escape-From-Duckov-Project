using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDispatcher : MonoBehaviour
{
    #region 인스펙터
    [Header("액션맵 설정 (테스트 용도)")]
    [SerializeField] private bool _startIngameActionMap = false;
    [SerializeField] private bool _startUIActionMap = false;
    [SerializeField] private bool _startLobbyActionMap = false;

    [Header("디버그 로그 설정")]
    [SerializeField] private bool _showSwitchActionMapLog = false;
    [SerializeField] private bool _showBindActionLog = false;
    [SerializeField] private bool _showInputLog = false;
    #endregion

    #region 내부 변수
    public static InputDispatcher Instance { get; private set; }

    private PlayerInputActions _inputActions;

    private bool _isReady = false;

    // 인게임 액션
    public event Action<Vector2> OnMove;
    public event Action OnDodge;
    public event Action<bool> OnRun;
    public event Action OnReload;
    public event Action OnInteract;
    public event Action OnFire;
    public event Action OnAim;
    public event Action OnMap;
    public event Action OnInventory;
    public event Action OnQuest;
    public event Action OnDiscard;
    public event Action OnMeleeSlot;
    public event Action OnWeaponSlot1;
    public event Action OnWeaponSlot2;
    public event Action OnQuickSlot3;
    public event Action OnQuickSlot4;
    public event Action OnQuickSlot5;
    public event Action OnQuickSlot6;
    public event Action OnQuickSlot7;
    public event Action OnQuickSlot8;
    public event Action OnPause;

    // UI 액션
    public event Action OnClickUI;
    public event Action OnReturnUI;
    public event Action OnPauseCancel;

    // 로비 액션
    public event Action OnPressAnyKey;
    public event Action OnBack;
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

        _inputActions = new PlayerInputActions();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            UnBindActions();

            if (_inputActions != null)
            {
                _inputActions.Disable();
                _inputActions.Dispose();
            }

            Instance = null;
        }
    }

    private void OnEnable()
    {
        BindActions();
    }

    private void OnDisable()
    {
        UnBindActions();
    }

    // 테스트용 스타트 함수
    private void Start()
    {
        if (_startIngameActionMap)
        {
            ChangeActionMap("Ingame");
        }

        else if (_startUIActionMap)
        {
            ChangeActionMap("Ui");
        }

        else if (_startLobbyActionMap)
        {
            ChangeActionMap("Lobby");
        }

        else
        {
            Debug.LogWarning("!!! 액션맵 설정 안되어있습니다 !!!");
            return;
        }
    }

    private void BindActions()
    {
        if (_isReady)
        {
            return;
        }

        _inputActions.Ingame.Move.performed += OnMovePerformed;
        _inputActions.Ingame.Move.canceled += OnMoveCanceled;
        _inputActions.Ingame.Dodge.performed += OnDodgePerformed;
        _inputActions.Ingame.Run.performed += OnRunPerformed;
        _inputActions.Ingame.Run.canceled += OnRunCanceled;
        _inputActions.Ingame.Reload.performed += OnReloadPerformed;
        _inputActions.Ingame.Interact.performed += OnInteractPerformed;
        _inputActions.Ingame.Fire.performed += OnFirePerformed;
        _inputActions.Ingame.Aim.performed += OnAimPerformed;
        _inputActions.Ingame.Map.performed += OnMapPerformed;
        _inputActions.Ingame.Inventory.performed += OnInventoryPerformed;
        _inputActions.Ingame.Quest.performed += OnQuestPerformed;
        _inputActions.Ingame.Discard.performed += OnDiscardPerformed;
        _inputActions.Ingame.MeleeSlot.performed += OnMeleeSlotPerformed;
        _inputActions.Ingame.WeaponSlot1.performed += OnWeaponSlot1Performed;
        _inputActions.Ingame.WeaponSlot2.performed += OnWeaponSlot2Performed;
        _inputActions.Ingame.QuickSlot3.performed += OnQuickSlot3Performed;
        _inputActions.Ingame.QuickSlot4.performed += OnQuickSlot4Performed;
        _inputActions.Ingame.QuickSlot5.performed += OnQuickSlot5Performed;
        _inputActions.Ingame.QuickSlot6.performed += OnQuickSlot6Performed;
        _inputActions.Ingame.QuickSlot7.performed += OnQuickSlot7Performed;
        _inputActions.Ingame.QuickSlot8.performed += OnQuickSlot8Performed;
        _inputActions.Ingame.Pause.performed += OnPausePerformed;
        _inputActions.Ui.ClickUI.performed += OnClickUIPerformed;
        _inputActions.Ui.ReturnUI.performed += OnReturnUIPerformed;
        _inputActions.Ui.PauseCancel.performed += OnPauseCancelPerformed;
        _inputActions.Lobby.PressAnyKey.performed += OnPressAnyKeyPerformed;
        _inputActions.Lobby.Back.performed += OnBackPerformed;

        if (_showBindActionLog)
        {
            Debug.Log("디스패처 액션 바인딩 완료");
        }

        _isReady = true;
    }

    private void UnBindActions()
    {
        if (!_isReady)
        {
            return;
        }

        _inputActions.Ingame.Move.performed -= OnMovePerformed;
        _inputActions.Ingame.Move.canceled -= OnMoveCanceled;
        _inputActions.Ingame.Dodge.performed -= OnDodgePerformed;
        _inputActions.Ingame.Run.performed -= OnRunPerformed;
        _inputActions.Ingame.Run.canceled -= OnRunCanceled;
        _inputActions.Ingame.Reload.performed -= OnReloadPerformed;
        _inputActions.Ingame.Interact.performed -= OnInteractPerformed;
        _inputActions.Ingame.Fire.performed -= OnFirePerformed;
        _inputActions.Ingame.Aim.performed -= OnAimPerformed;
        _inputActions.Ingame.Map.performed -= OnMapPerformed;
        _inputActions.Ingame.Inventory.performed -= OnInventoryPerformed;
        _inputActions.Ingame.Quest.performed -= OnQuestPerformed;
        _inputActions.Ingame.Discard.performed -= OnDiscardPerformed;
        _inputActions.Ingame.MeleeSlot.performed -= OnMeleeSlotPerformed;
        _inputActions.Ingame.WeaponSlot1.performed -= OnWeaponSlot1Performed;
        _inputActions.Ingame.WeaponSlot2.performed -= OnWeaponSlot2Performed;
        _inputActions.Ingame.QuickSlot3.performed -= OnQuickSlot3Performed;
        _inputActions.Ingame.QuickSlot4.performed -= OnQuickSlot4Performed;
        _inputActions.Ingame.QuickSlot5.performed -= OnQuickSlot5Performed;
        _inputActions.Ingame.QuickSlot6.performed -= OnQuickSlot6Performed;
        _inputActions.Ingame.QuickSlot7.performed -= OnQuickSlot7Performed;
        _inputActions.Ingame.QuickSlot8.performed -= OnQuickSlot8Performed;
        _inputActions.Ingame.Pause.performed -= OnPausePerformed;
        _inputActions.Ui.ClickUI.performed -= OnClickUIPerformed;
        _inputActions.Ui.ReturnUI.performed -= OnReturnUIPerformed;
        _inputActions.Ui.PauseCancel.performed -= OnPauseCancelPerformed;
        _inputActions.Lobby.PressAnyKey.performed -= OnPressAnyKeyPerformed;
        _inputActions.Lobby.Back.performed -= OnBackPerformed;

        if (_showBindActionLog)
        {
            Debug.Log("디스패처 액션 언바인딩 완료");
        }

        _isReady = false;
    }

    #region 액션 콜백 함수
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();

        if (_showInputLog)
        {
            Debug.Log($"Input Move = {v}");
        }

        OnMove?.Invoke(v);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        Vector2 v = Vector2.zero;

        if (_showInputLog)
        {
            Debug.Log($"Input Move = {v}");
        }

        OnMove?.Invoke(v);
    }

    private void OnDodgePerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Dodge");
        }

        OnDodge?.Invoke();
    }

    private void OnRunPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Run");
        }

        OnRun?.Invoke(true);
    }

    private void OnRunCanceled(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log("Cancel Run");
        }

        OnRun?.Invoke(false);
    }

    private void OnReloadPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Reload");
        }

        OnReload?.Invoke();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Interact");
        }

        OnInteract?.Invoke();
    }

    private void OnFirePerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Fire");
        }

        OnFire?.Invoke();
    }

    private void OnAimPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Aim");
        }

        OnAim?.Invoke();
    }

    private void OnMapPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Map");
        }

        OnMap?.Invoke();
    }

    private void OnInventoryPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Inventory");
        }

        OnInventory?.Invoke();
    }

    private void OnQuestPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Quest");
        }

        OnQuest?.Invoke();
    }

    private void OnDiscardPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Discard");
        }

        OnDiscard?.Invoke();
    }

    private void OnMeleeSlotPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input MeleeSlot");
        }

        OnMeleeSlot?.Invoke();
    }

    private void OnWeaponSlot1Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input WeaponSlot1");
        }

        OnWeaponSlot1?.Invoke();
    }

    private void OnWeaponSlot2Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input WeaponSlot2");
        }

        OnWeaponSlot2?.Invoke();
    }

    private void OnQuickSlot3Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot3");
        }

        OnQuickSlot3?.Invoke();
    }

    private void OnQuickSlot4Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot4");
        }

        OnQuickSlot4?.Invoke();
    }

    private void OnQuickSlot5Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot5");
        }

        OnQuickSlot5?.Invoke();
    }

    private void OnQuickSlot6Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot6");
        }

        OnQuickSlot6?.Invoke();
    }

    private void OnQuickSlot7Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot7");
        }

        OnQuickSlot7?.Invoke();
    }

    private void OnQuickSlot8Performed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input QuickSlot8");
        }

        OnQuickSlot8?.Invoke();
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Pause");
        }

        OnPause?.Invoke();
    }

    private void OnClickUIPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input ClickUI");
        }

        OnClickUI?.Invoke();
    }

    private void OnReturnUIPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input ReturnUI");
        }

        OnReturnUI?.Invoke();
    }

    private void OnPauseCancelPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input PauseCancel");
        }

        OnPauseCancel?.Invoke();
    }

    private void OnPressAnyKeyPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input PressAnyKey");
        }

        OnPressAnyKey?.Invoke();
    }

    private void OnBackPerformed(InputAction.CallbackContext ctx)
    {
        if (_showInputLog)
        {
            Debug.Log($"Input Back");
        }
        OnBack?.Invoke();
    }
    #endregion

    #region 외부 호출 함수
    public void ChangeActionMap(string actionMapName)
    {
        if (_inputActions == null)
        {
            Debug.LogWarning("_inputActions == null");
            return;
        }

        _inputActions.Disable();

        switch (actionMapName)
        {
            case "Ingame":
                _inputActions.Ingame.Enable();
                break;

            case "Ui":
                _inputActions.Ui.Enable();
                break;

            case "Lobby":
                _inputActions.Lobby.Enable();
                break;

            default:
                Debug.LogWarning($"액션맵 이름 확인 필요: {actionMapName}");
                break;
        }

        if (_showSwitchActionMapLog)
        {
            Debug.Log($"액션맵 전환 완료: {actionMapName}");
        }
    }

    public void DisableInputActions()
    {
        _inputActions.Disable();

        if (_showSwitchActionMapLog)
        {
            Debug.Log($"액션맵 비활성화 완료");
        }
    }
    #endregion
}