using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private SceneDataSO _sceneDataSO;
    [SerializeField] private GameObject _canvas;

    [SerializeField] private float _minLoadingTime = 1.5f;
    #endregion

    #region 내부 변수
    public static SceneLoader Instance { get; private set; }
    private SceneTransitionUI _sceneTransitionUI;
    private bool _isLoading = false;
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

        if (_sceneDataSO == null)
        {
            Debug.LogError("씬 데이터 SO가 할당되지 않았습니다.");
        }

        _sceneTransitionUI = GetComponentInChildren<SceneTransitionUI>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        _canvas.SetActive(false);
        _sceneTransitionUI.Init();
        _sceneTransitionUI.ResultInit();
    }

    private IEnumerator CoLoadScene(string sceneName, string nextActionMap, bool clickDirection = false)
    {
        _sceneTransitionUI.Init();
        _sceneTransitionUI.ResultInit();
        _isLoading = true;

        string currentScene = SceneManager.GetActiveScene().name;

        InputDispatcher.Instance.DisableInputActions();
        CursorManager.Instance.SetCursorByScene();
        SoundManager.Instance.FadeOutBGM(0.9f);

        _canvas.SetActive(true);

        yield return _sceneTransitionUI.CircleIn().WaitForCompletion();
        _sceneTransitionUI.ScreenTurnOff();
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;
        _sceneTransitionUI.SetLoadingTextUI(true);

        // 여기서 로딩 씬별로 ui 추가?
        SetLoadingUI(currentScene, sceneName, true);

        float timer = 0f;

        while (op.progress < 0.9f || timer < _minLoadingTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _sceneTransitionUI.SetLoadingTextUI(false);

        yield return new WaitForSeconds(0.5f);

        if (clickDirection)
        {
            _sceneTransitionUI.SetClickUI(true);

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));


            op.allowSceneActivation = true;

            yield return null;

            SoundManager.Instance.PlaySFX("Lobby_Click");

            _sceneTransitionUI.SetClickUI(false);

            SetLoadingUI(currentScene, sceneName, false);

            yield return new WaitForSeconds(1f);
        }

        else
        {
            op.allowSceneActivation = true;

            SetLoadingUI(currentScene, sceneName, false);

            yield return new WaitForSeconds(1f);
        }

        switch (sceneName)
        {
            case "Base":
                SoundManager.Instance.PlaySFX("Base_Start");
                SoundManager.Instance.PlayBGM("Base_BGM1", 0.9f, 7f);
                break;

            case "Basement":
                SoundManager.Instance.PlayBGM("Basement_BGM", 1f);
                break;

            case "GroundZero":
                SoundManager.Instance.PlayBGM("GroundZero_BGM", 1.5f);
                break;

            default:
                Debug.Log($"배경음 재생 실패 [{sceneName}]");
                break;
        }

        CursorManager.Instance.SetCursorByScene(nextActionMap);

        yield return _sceneTransitionUI.CircleOut().WaitForCompletion();

        InputDispatcher.Instance.ChangeActionMap(nextActionMap);
        _canvas.SetActive(false);
        _sceneTransitionUI.Init(); // 혹시 모를 초기화??
        _sceneTransitionUI.ResultInit(); // 혹시 모를 초기화??
        _isLoading = false;
    }

    private void SetLoadingUI(string currentScene, string nextScene, bool isActive)
    {
        if ((currentScene == "Base" || currentScene == "Basement") && nextScene == "GroundZero")
        {
            _sceneTransitionUI.SetStartLoadingUI(isActive);
        }

        else if (currentScene == "GroundZero" && nextScene == "Base")
        {
            /*
            플레이어가 죽었니? => 사망 텍스트 + 배경
            if (PlayerDead)
            {
                _sceneTransitionUI.SetDeadLoadingUI(isActive)

                PlayerDead 플래그 false로 바꿔주기
            }

            else
            {

            }
            */

            // else 부분 (플레이어가 탈출했을때)
            _sceneTransitionUI.SetEscapeLoadingUI(isActive);
        }
    }

    #region 외부 호출 함수
    public void LoadScene(string sceneID, string nextActionMap, bool clickDirection = false)
    {
        if (_isLoading)
        {
            Debug.LogWarning("이미 씬이 로드 중입니다.");
            return;
        }

        string sceneName = _sceneDataSO.GetSceneNameById(sceneID);

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"씬 ID [{sceneID}]에 해당하는 씬 이름을 찾을 수 없습니다.");
            return;
        }

        StartCoroutine(CoLoadScene(sceneName, nextActionMap, clickDirection));
    }

    public void ShowResultUI(PlayerDataSO player, int gainExp, bool isEscape)
    {
        _canvas.SetActive(true);
        InputDispatcher.Instance.ChangeActionMap("Lobby");
        CursorManager.Instance.SetCursorByScene("Lobby");
        _sceneTransitionUI.ResultUI(player, gainExp, isEscape);
    }
    #endregion
}