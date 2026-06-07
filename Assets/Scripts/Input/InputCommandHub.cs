using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommandHub : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private bool _showBindLog = false;
    #endregion

    #region 내부 변수
    public static InputCommandHub Instance { get; private set; }
    private readonly Dictionary<string, IInputCommand> _commands = new Dictionary<string, IInputCommand>();
    private readonly Dictionary<string, object> _valueCommands = new Dictionary<string, object>();
    private InputDispatcher _dispatcher;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _dispatcher = GetComponent<InputDispatcher>();

        if (_dispatcher == null)
        {
            Debug.LogError("InputCommandHub _dispatcher == null");
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            UnBindInputEvents();
            Instance = null;
        }
    }

    private void OnEnable()
    {
        BindInputEvents();
    }

    private void BindInputEvents()
    {
        _dispatcher.OnMove += HandleMove;
        _dispatcher.OnRun += HandleRun;
        _dispatcher.OnDodge += HandleDodge;
        _dispatcher.OnFire += HandleFire;
        _dispatcher.OnAim += HandleAim;
        _dispatcher.OnReload += HandleReload;
        _dispatcher.OnPause += HandlePause;
        _dispatcher.OnInventory += HandleInventory;
        _dispatcher.OnInteract += HandleInteract;
        _dispatcher.OnMap += HandleMap;
        _dispatcher.OnQuest += HandleQuest;
        _dispatcher.OnPressAnyKey += HandlePressAnyKey;
        _dispatcher.OnBack += HandleBack;
        _dispatcher.OnPauseCancel += HandlePauseCancel;
        _dispatcher.OnInventoryCancel += HandleInventoryCancel;
        _dispatcher.OnMapCancel += HandleMapCancel;
        _dispatcher.OnQuestCancel += HandleQuestCancel;
        _dispatcher.OnInteractionCancel += HandleInteractionCancel;

        if (_showBindLog)
        {
            Debug.Log("허브 입력 이벤트 바인드 완료");
        }
    }

    private void UnBindInputEvents()
    {
        _dispatcher.OnMove -= HandleMove;
        _dispatcher.OnRun -= HandleRun;
        _dispatcher.OnDodge -= HandleDodge;
        _dispatcher.OnFire -= HandleFire;
        _dispatcher.OnAim -= HandleAim;
        _dispatcher.OnReload -= HandleReload;
        _dispatcher.OnPause -= HandlePause;
        _dispatcher.OnInventory -= HandleInventory;
        _dispatcher.OnInteract -= HandleInteract;
        _dispatcher.OnMap -= HandleMap;
        _dispatcher.OnQuest -= HandleQuest;
        _dispatcher.OnPressAnyKey -= HandlePressAnyKey;
        _dispatcher.OnBack -= HandleBack;
        _dispatcher.OnPauseCancel -= HandlePauseCancel;
        _dispatcher.OnInventoryCancel -= HandleInventoryCancel;
        _dispatcher.OnMapCancel -= HandleMapCancel;
        _dispatcher.OnQuestCancel -= HandleQuestCancel;
        _dispatcher.OnInteractionCancel -= HandleInteractionCancel;

        if (_showBindLog)
        {
            Debug.Log("허브 입력 이벤트 언바인드 완료");
        }
    }

    #region 핸들러 함수
    private void HandleMove(Vector2 direction)
    {
        ExecuteValue("Player.Move", direction);
    }

    private void HandleRun(bool isPress)
    {
        ExecuteValue("Player.Run", isPress);
    }

    private void HandleDodge()
    {
        Execute("Player.Dodge");
    }

    private void HandleFire(bool isPress)
    {
        ExecuteValue("Player.Fire", isPress);
    }

    private void HandleAim(bool isPress)
    {
        ExecuteValue("Player.Aim", isPress);
    }

    private void HandleReload()
    {
        Execute("Player.Reload");
    }

    private void HandlePause()
    {
        Execute("Player.Pause");
    }

    private void HandleInventory()
    {
        Execute("Player.Inventory");
    }

    private void HandleInteract()
    {
        Execute("Player.Interact");
    }

    private void HandleMap()
    {
        Execute("Player.Map");
    }

    private void HandleQuest()
    {
        Execute("Player.Quest");
    }

    private void HandlePressAnyKey()
    {
        Execute("Lobby.PressAnyKey");
    }

    private void HandleBack()
    {
        Execute("Lobby.Back");
    }

    private void HandlePauseCancel()
    {
        Execute("Ui.PauseCancel");
    }

    private void HandleInventoryCancel()
    {
        Execute("Ui.InventoryCancel");
    }

    private void HandleMapCancel()
    {
        Execute("Ui.MapCancel");
    }

    private void HandleQuestCancel()
    {
        Execute("Ui.QuestCancel");
    }

    private void HandleInteractionCancel()
    {
        Execute("Ui.InteractionCancel");
    }
    #endregion

    private void Execute(string key)
    {
        if (_commands.TryGetValue(key, out IInputCommand command) == false || command == null)
        {
            return;
        }

        command.Execute();
    }

    private void ExecuteValue<T>(string key, T value)
    {
        if (_valueCommands.TryGetValue(key, out object obj) == false || obj == null)
        {
            return;
        }

        if (obj is IInputCommand<T> command)
        {
            command.Execute(value);
        }
    }

    #region 외부 호출 함수
    public void RegisterCommands(string key, IInputCommand command)
    {
        _commands[key] = command;
    }

    public void RegisterValueCommands<T>(string key, IInputCommand<T> command)
    {
        _valueCommands[key] = command;
    }
    #endregion
}