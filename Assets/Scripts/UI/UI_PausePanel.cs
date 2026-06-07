using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_PausePanel : MonoBehaviour
{
	#region 인스펙터
	[SerializeField] private Button _resumeButton;
	[SerializeField] private Button _settingButton;
	[SerializeField] private Button _mainMenuButton;
	[SerializeField] private Button _exitGameButton;

    [Header("연출 설정")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    [Header("세팅 패널 참조")]
    [SerializeField] private SettingPanel _settingPanel; 
    #endregion

    private void Awake()
    {
        _resumeButton.onClick.AddListener(OnClickResume);
        _settingButton.onClick.AddListener(OnClickSetting);
        _mainMenuButton.onClick.AddListener(OnClickMainMenu);
        _exitGameButton.onClick.AddListener(OnClickExitGame);
        _canvasGroup.alpha = 0f;
    }

    private void OnClickResume()
    {
        IngameUIManager.Instance.ClosePausePanel();
    }

    private void OnClickSetting()
    {
        _settingPanel.Show();
    }

    private void OnClickMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("Lobby", "Lobby");
    }

    private void OnClickExitGame()
    {
        Application.Quit();
    }

    public void Show()
    {
        InputDispatcher.Instance.ChangeActionMap("Ui");
        CursorManager.Instance.SetCursorByScene("Ui");
        _settingPanel.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeDuration).SetUpdate(true);
        Time.timeScale = 0f;
    }

    public void Hide()
    {        
        Time.timeScale = 1f;
        InputDispatcher.Instance.ChangeActionMap("Ingame");
        CursorManager.Instance.SetCursorByScene("Ingame");
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeDuration).SetUpdate(true).OnComplete(() => this.gameObject.SetActive(false));
        _settingPanel.gameObject.SetActive(false);
    }
}