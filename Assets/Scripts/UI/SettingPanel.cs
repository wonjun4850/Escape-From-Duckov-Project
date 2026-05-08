using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    #region 인스펙터
    [Header("세팅패널 연결")]
    [SerializeField] private Button[] _buttons;
    [SerializeField] private GameObject[] _list;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("색 설정")]
    [SerializeField] private Color _activeColor = new Color(0, 0, 0, 1);
    [SerializeField] private Color _inactiveColor = new Color(0, 0, 0, 0.5f);

    [Header("세팅패널 페이드 연출시간 설정")]
    [SerializeField] private float _fadeDuration = 0.5f;
    #endregion

    private void Awake()
    {
        if (_buttons == null || _buttons.Length == 0 || _list == null || _list.Length == 0 || _canvasGroup == null)
        {
            Debug.LogError("SettingPanel: 인스펙터 확인");
            return;
        }

        InitTab();
    }

    private void InitTab()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].GetComponent<Image>().color = (i == 0) ? _activeColor : _inactiveColor;

            _list[i].SetActive(i == 0);
        }
    }

    #region 버튼 함수
    public void OnClickTab(int index)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            Image btn = _buttons[i].GetComponent<Image>();

            btn.color = (i == index) ? _activeColor : _inactiveColor;

            _list[i].SetActive(i == index);
        }
    }
    #endregion

    #region 외부 호출 함수
    public void Show()
    {
        this.gameObject.SetActive(true);

        _canvasGroup.alpha = 0f;

        _canvasGroup.DOFade(1f, _fadeDuration);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() => this.gameObject.SetActive(false));
    }
    #endregion
}