using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_HUDTransition : MonoBehaviour
{
    #region 인스펙터
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("페이드 시간 설정")]
    [SerializeField] private float _fadeDuration = 0.25f;
    #endregion

    #region 외부 호출 함수
    public void Show()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeDuration).SetUpdate(true);
    }

    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeDuration).SetUpdate(true);
    }
    #endregion
}