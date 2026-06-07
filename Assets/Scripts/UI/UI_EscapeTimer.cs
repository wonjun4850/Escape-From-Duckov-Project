using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_EscapeTimer : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _timerText;

    [SerializeField] private RectMask2D _rectMask;
    [SerializeField] private RectTransform _slashPattern;

    [Header("Mask Padding Right 값과 동일하게 설정")]
    [SerializeField] private float _paddingRight = 200f;

    [Header("셰이더 적용된 백그라운드")]
    [SerializeField] private Image _backgroundShader;
    [SerializeField] private string _shaderRef = "_ClipAmount";
    [SerializeField, Range(0f, 1f)] private float _shaderStartValue = 1f;
    [SerializeField, Range(0f, 1f)] private float _shaderEndValue = 0f;

    [Header("연출 설정")]
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _slashMoveDistance = 30f;
    [SerializeField] private float _slashMoveDuration = 0.5f;
    #endregion

    #region 내부 변수
    private Tweener _slashTween;
    private Material _shaderMat;
    private float _escapeTime;
    private float _remainingTime;
    #endregion

    private void Awake()
    {
        _canvasGroup.alpha = 0f;

        _slashTween = _slashPattern.DOAnchorPosX(_slashMoveDistance, _slashMoveDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetAutoKill(false)
                .Pause();

        _shaderMat = _backgroundShader.material;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _slashTween?.Kill();
    }

    private void SetRectMaskPaddingRight(float paddingRight)
    {
        Vector4 p = _rectMask.padding;
        p.z = paddingRight;
        _rectMask.padding = p;
    }

    #region 외부 호출 함수
    public void StartUI(float escapeTime)
    {
        _escapeTime = escapeTime;
        _remainingTime = escapeTime;

        SetRectMaskPaddingRight(_paddingRight);
        gameObject.SetActive(true);

        _slashTween?.Play();
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeDuration);
    }

    public void EndUI()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _slashTween?.Pause();
        });
    }

    public void UpdateUI(float timer)
    {
        _remainingTime = timer;

        float currentTime = Mathf.Max(0f, _remainingTime);

        int min = Mathf.FloorToInt(currentTime / 60f);
        int sec = Mathf.FloorToInt(currentTime % 60f);
        int millisec = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        _timerText.SetText($"{min:00}:{sec:00}.<size=50%>{millisec:000}</size>");

        float progressRatio = (_escapeTime - currentTime) / _escapeTime;
        float padding = Mathf.Lerp(_paddingRight, 0f, progressRatio);
        SetRectMaskPaddingRight(padding);

        float shaderValue = Mathf.Lerp(_shaderStartValue, _shaderEndValue, progressRatio);
        _shaderMat.SetFloat(_shaderRef, shaderValue);
    }
    #endregion
}