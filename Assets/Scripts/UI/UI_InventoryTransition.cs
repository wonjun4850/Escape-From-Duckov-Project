using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI_InventoryTransition : MonoBehaviour
{
    #region 인스펙터
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("창고 패널")]
    [SerializeField] private GameObject _storagePanel;

    [Header("이동 설정")]
    [SerializeField] private RectTransform _inventoryRect;
    [SerializeField] private Vector2 _inventoryStartPos;
    [SerializeField] private Vector2 _inventoryEndPos;

    [SerializeField] private RectTransform _storageRect;
    [SerializeField] private Vector2 _storageStartPos;
    [SerializeField] private Vector2 _storageEndPos;

    [Header("시간 설정")]
    [SerializeField] private float _duration = 0.25f;
    #endregion

    #region 내부 변수
    private Sequence _sq;
    #endregion

    private void Awake()
    {
        _inventoryRect.anchoredPosition = _inventoryStartPos;
        _storageRect.anchoredPosition = _storageStartPos;
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
        _sq.Join(_inventoryRect.DOAnchorPos(_inventoryEndPos, _duration));

        if (_storagePanel.activeSelf)
        {
            _sq.Join(_storageRect.DOAnchorPos(_storageEndPos, _duration));
        }                
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
        _sq.Join(_inventoryRect.DOAnchorPos(_inventoryStartPos, _duration));

        if (_storagePanel.activeSelf)
        {
            _sq.Join(_storageRect.DOAnchorPos(_storageStartPos, _duration));
        }      

        _sq.OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            _sq = null;
        });
    }

    public void SetStorageActiveByScene(bool isActive)
    {
        if (_storagePanel != null)
        {
            _storagePanel.SetActive(isActive);
        }
    }
    #endregion
}