using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    }

    private IEnumerator CoLoadScene(string sceneName, string nextActionMap, bool clickDirection = false)
    {
        _isLoading = true;        

        InputDispatcher.Instance.DisableInputActions();
        CursorManager.Instance.SetCursorByScene();
        SoundManager.Instance.FadeOutBGM(0.9f);

        _canvas.SetActive(true);

        yield return _sceneTransitionUI.CircleIn().WaitForCompletion();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;
        _sceneTransitionUI.SetLoadingUI(true);

        float timer = 0f;

        while (op.progress < 0.9f || timer < _minLoadingTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _sceneTransitionUI.SetLoadingUI(false);

        yield return new WaitForSeconds(0.5f);

        if (clickDirection)
        {
            _sceneTransitionUI.SetClickUI(true);

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            op.allowSceneActivation = true;

            yield return null;

            _sceneTransitionUI.SetClickUI(false);

            yield return new WaitForSeconds(0.5f);
        }

        else
        {
            op.allowSceneActivation = true;

            yield return new WaitForSeconds(0.5f);
        }

        CursorManager.Instance.SetCursorByScene(nextActionMap);

        //SoundManager.Instance.PlaySFX("Base_Start");
        SoundManager.Instance.PlayBGM("Base_BGM1", 0.9f);

        yield return _sceneTransitionUI.CircleOut().WaitForCompletion();

        InputDispatcher.Instance.ChangeActionMap(nextActionMap);
        _canvas.SetActive(false);
        //_sceneTransitionUI.Init(); 초기화가 필요할지도..?
        _isLoading = false;
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
    #endregion
}