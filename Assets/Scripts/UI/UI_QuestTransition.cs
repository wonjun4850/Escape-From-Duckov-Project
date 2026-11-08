using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestTransition : MonoBehaviour
{
    #region 인스펙터
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("이동 설정")]
    [SerializeField] private RectTransform _questRect;
    [SerializeField] private Vector2 _questStartPos;
    [SerializeField] private Vector2 _questEndPos;

    [Header("시간 설정")]
    [SerializeField] private float _duration = 0.25f;
    #endregion

    #region 내부 변수
    private Sequence _sq;
    #endregion

    private void Awake()
    {
        _questRect.anchoredPosition = _questStartPos;
        _canvasGroup.alpha = 0f;
    }

    #region 외부 호출 함수
    public void Show()
    {
        _sq?.Kill();

        InputDispatcher.Instance.ChangeActionMap("Ui");
        CursorManager.Instance.SetCursorByScene("Ui");
        SoundManager.Instance.PlaySFX("Inventory_Open");

        _canvasGroup.alpha = 0f;
        this.gameObject.SetActive(true);

        _sq = DOTween.Sequence();
        _sq.SetUpdate(true);
        _sq.Join(_canvasGroup.DOFade(1f, _duration));
        _sq.Join(_questRect.DOAnchorPos(_questEndPos, _duration));
    }

    public void Hide()
    {
        _sq?.Kill();

        InputDispatcher.Instance.ChangeActionMap("Ingame");
        CursorManager.Instance.SetCursorByScene("Ingame");
        SoundManager.Instance.PlaySFX("Inventory_Close");

        _sq = DOTween.Sequence();
        _sq.SetUpdate(true);
        _sq.Join(_canvasGroup.DOFade(0f, _duration));
        _sq.Join(_questRect.DOAnchorPos(_questStartPos, _duration));

        _sq.OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            _sq = null;
        });
    }
    #endregion
}
