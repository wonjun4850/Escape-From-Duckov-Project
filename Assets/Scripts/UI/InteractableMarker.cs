using DG.Tweening;
using UnityEngine;

public class InteractableMarker : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _fillCircle;
    [SerializeField] private RectTransform _emptyCircle;

    [Header("연출 설정")]
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _RoopDuration = 0.8f;
    [SerializeField] private float _scaleValue = 0.8f;
    #endregion

    #region 내부 변수
    private Sequence _sq;
    private Transform _target;
    private Camera _camera;
    #endregion

    void LateUpdate()
    {
        if (_target == null || _camera == null)
        {
            return;
        }

        this.transform.position = _camera.WorldToScreenPoint(_target.position);
    }

    #region 외부 호출 함수
    public void Init(Transform target, out bool isSuccess)
    {
        isSuccess = false;

        if (target == null)
        {
            Debug.LogError("마커 타겟 null");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("마커 메인캠 null");
            return;
        }

        if (_canvasGroup == null || _fillCircle == null || _emptyCircle == null)
        {
            Debug.LogError("마커 참조 null");
            return;
        }

        _target = target;
        _camera = Camera.main;

        _canvasGroup.alpha = 0f;
        _fillCircle.localScale = Vector3.one * (_scaleValue * 0.25f);
        _emptyCircle.localScale = Vector3.one * _scaleValue;
        isSuccess = true;
    }

    public void PlayMarkerSequence()
    {
        _sq?.Kill();

        _sq = DOTween.Sequence();

        float fillEnter = _scaleValue * 0.125f;
        float emptyEnter = _scaleValue * 0.4f;

        _sq.Append(_canvasGroup.DOFade(1f, _duration));
        _sq.Join(_fillCircle.DOScale(fillEnter, _duration));
        _sq.Join(_emptyCircle.DOScale(emptyEnter, _duration));

        _sq.OnComplete(() =>
        {
            float loopFillScale = _scaleValue * 0.15f;
            float loopEmptyScale = _scaleValue * 0.35f;

            _fillCircle.DOScale(loopFillScale, _RoopDuration).SetLoops(-1, LoopType.Yoyo);
            _emptyCircle.DOScale(loopEmptyScale, _RoopDuration).SetLoops(-1, LoopType.Yoyo);
        });
    }

    public void ExitMarkerSequence()
    {
        _fillCircle?.DOKill();
        _emptyCircle?.DOKill();

        _sq?.Kill();

        _sq = DOTween.Sequence();

        float fillExit = _scaleValue * 0.25f;
        float emptyExit = _scaleValue * 0.8f;

        _sq.Append(_canvasGroup.DOFade(0f, _duration));
        _sq.Join(_fillCircle.DOScale(fillExit, _duration));
        _sq.Join(_emptyCircle.DOScale(emptyExit, _duration));

        _sq.OnComplete(() =>
        {
            Destroy(this.gameObject);
        });
    }
    #endregion
}