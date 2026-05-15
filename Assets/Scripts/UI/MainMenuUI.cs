using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	#region 인스펙터
	[Header("메인 메뉴 버튼 연결")]
	[SerializeField] private GameObject _buttonAll;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("세팅 패널 연결")]
	[SerializeField] private SettingPanel _settingPanel;

    [Header("메뉴버튼 페이드 연출 시간 설정")]
    [SerializeField] private float _fadeDuration = 0.5f;
    #endregion

    private void Awake()
    {
        if (_buttonAll == null || _settingPanel == null || _canvasGroup == null)
		{
			Debug.LogError("MainMenuUI: 할당되지 않은 UI 요소가 있습니다.");
			return;
        }

		_buttonAll.SetActive(false);
        _settingPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _buttonAll.SetActive(true);
        _canvasGroup.alpha = 1f;
        InputCommandHub.Instance.RegisterCommands("Lobby.Back", new SimpleCommand(Back));
    }

    private void OnDisable()
    {
        if (InputCommandHub.Instance != null)
        {
            InputCommandHub.Instance.RegisterCommands("Lobby.Back", null);
        }
    }

    private void Back()
	{
        if (_settingPanel.gameObject.activeSelf)
        {
            OnClickCloseSetting();
        }
    }

	#region 버튼 함수
	public void OnClickStart()
	{
		SceneLoader.Instance.LoadScene("Base1", "Ingame", true);
    }

	public void OnClickSetting()
	{
        _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() => _buttonAll.SetActive(false));
        _settingPanel.Show();
    }

	public void OnClickCloseSetting()
	{
        _buttonAll.SetActive(true);
        _canvasGroup.DOFade(1f, _fadeDuration);
        _settingPanel.Hide();
    }

    public void OnClickURL()
	{
		Debug.Log("URL 열기");
		//Application.OpenURL("");
    }

	public void OnClickExit()
	{
		Debug.Log("게임 종료");
		Application.Quit();
    }
	#endregion
}