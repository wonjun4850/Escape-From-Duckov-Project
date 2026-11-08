using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    #region 인스펙터
    [Header("연출 설정")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 0.25f;
    #endregion

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        InputDispatcher.Instance.ChangeActionMap("Ui");
        CursorManager.Instance.SetCursorByScene("Ui");
        SoundManager.Instance.PlaySFX("Map_Open");
        this.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeDuration).SetUpdate(true);
    }

    public void Hide()
    {
        InputDispatcher.Instance.ChangeActionMap("Ingame");
        CursorManager.Instance.SetCursorByScene("Ingame");
        SoundManager.Instance.PlaySFX("Map_Close");
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeDuration).SetUpdate(true).OnComplete(() => this.gameObject.SetActive(false));
    }
}
