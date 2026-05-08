using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionUI : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 전환 연출 UI")]
    [SerializeField] private CanvasGroup _sceneChangeUI;
    [SerializeField] private RectTransform _changeCircle;
    [SerializeField] private float _maxScale = 30f;
    [SerializeField] private float _midScale = 10f;
    [SerializeField] private float _minScale = 0f;
    [SerializeField] private float _duration = 2f;

    [Header("로딩중 UI")]
    [SerializeField] private CanvasGroup _loadingUI;
    [SerializeField] private float _loadingFadeDuration = 0.5f;

    [Header("클릭 UI")]
    [SerializeField] private CanvasGroup _clickUI;
    [SerializeField] private float _clickFadeDuration = 0.5f;
    [SerializeField] private RectTransform _circleAll; // 스케일값 연출용
    [SerializeField] private CanvasGroup _circleAllCanvas; // 알파값 연출용
    [SerializeField] private Image _edgeCircle;
    [SerializeField] private CanvasGroup _fillCircle;
    [SerializeField] private TextMeshProUGUI _clickText;
    #endregion

    #region 외부 호출 함수
    public void Init()
    {
        _sceneChangeUI.alpha = 0f;
        _loadingUI.alpha = 0f;
        _clickUI.alpha = 0f;
        _changeCircle.localScale = Vector3.one * _maxScale;

        _sceneChangeUI.gameObject.SetActive(false);
        _loadingUI.gameObject.SetActive(false);
        _clickUI.gameObject.SetActive(false);
    }

    public Tween CircleIn()
    {
        _sceneChangeUI.gameObject.SetActive(true);
        _sceneChangeUI.alpha = 1f;

        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(_changeCircle.DOScale(_midScale, _duration * 0.6f).SetEase(Ease.OutBack));
        sq.Append(_changeCircle.DOScale(_minScale, _duration * 0.3f).SetEase(Ease.InExpo));
        return sq;
    }

    public Tween CircleOut()
    {
        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(_changeCircle.DOScale(_midScale, _duration * 0.6f).SetEase(Ease.OutBack));
        sq.Append(_changeCircle.DOScale(_maxScale, _duration * 0.3f).SetEase(Ease.InExpo));
        sq.OnComplete(() => _sceneChangeUI.gameObject.SetActive(false));
        return sq;
    }

    public void SetLoadingUI(bool isActive)
    {
        if (!_loadingUI.gameObject.activeSelf)
        {
            _loadingUI.gameObject.SetActive(true);
        }

        if (isActive)
        {
            _loadingUI.DOFade(1f, _loadingFadeDuration).SetUpdate(true);
        }

        else
        {
            _loadingUI.DOFade(0f, _loadingFadeDuration).SetUpdate(true).OnComplete(() => _loadingUI.gameObject.SetActive(false));
        }
    }

    public void SetClickUI(bool isActive)
    {
        if (!_clickUI.gameObject.activeSelf)
        {
            _clickUI.gameObject.SetActive(true);
        }

        if (isActive)
        {
            _fillCircle.alpha = 1f;

            _clickUI.DOFade(1f, _clickFadeDuration).SetUpdate(true).OnComplete(() =>
            {
                _circleAll.DOScale(1.35f, 1.25f).SetLoops(-1);
                _circleAllCanvas.DOFade(0f, 1.25f).SetLoops(-1);
            });
        }

        else
        {
            _clickText.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0.15f), 0.3f, 5, 1f);

            _clickUI.DOFade(0f, _clickFadeDuration).SetUpdate(true).OnComplete(() =>
            {
                _circleAll.DOKill();
                _circleAllCanvas.DOKill();
                _fillCircle.alpha = 0f;
                _clickUI.gameObject.SetActive(false);
            });
        }
    }
    #endregion
}