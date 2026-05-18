using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_StaminaGauge : MonoBehaviour
{
    #region 인스펙터
    [Header("위치 조절")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, 0);

    [Header("게이지 연결")]
    [SerializeField] private Image[] _fillGauges;

    [Header("색 설정")]
    [SerializeField] private Color _normalColor = new Color();
    [SerializeField] private Color _lowColor = new Color();
    #endregion

    #region 내부 변수
    private StaminaSystem _staminaSystem;
    private CanvasGroup _canvasGroup;
    private Transform _playerTr;
    private Camera _camera;

    private bool _isShowing = false;
    #endregion

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (_staminaSystem != null)
        {
            _staminaSystem.OnStaminaChanged -= RefreshUI;
        }
    }

    private void RefreshUI(float ratio)
    {
        for (int i = 0; i < _fillGauges.Length; i++)
        {
            _fillGauges[i].fillAmount = ratio;
            _fillGauges[i].color = _staminaSystem.IsStaminaLow() ? _lowColor : _normalColor;
        }

        bool show = ratio < 0.999f;

        if (show && !_isShowing)
        {
            _isShowing = true;
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1f, 0.25f);
        }

        else if (!show && _isShowing)
        {
            _isShowing = false;
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, 0.25f);
        }
    }

    private void LateUpdate()
    {
        if (_playerTr == null || !_isShowing) return;

        if (_camera == null)
        {
            _camera = Camera.main;
        }

        Vector3 screenPos = _camera.WorldToScreenPoint(_playerTr.position + _offset);
        transform.position = screenPos;
    }

    #region 외부 호출 함수
    public void SetUp(StaminaSystem stamina, Transform playerTr)
    {
        UnBind();

        _staminaSystem = stamina;
        _playerTr = playerTr;
        _camera = Camera.main;

        if (_staminaSystem != null)
        {
            _staminaSystem.OnStaminaChanged += RefreshUI;
            RefreshUI(_staminaSystem.GetStaminaRatio());
        }

        else
        {
            Debug.LogError("_staminaSystem = null 확인 필요");
        }

        if (_playerTr == null)
        {
            Debug.LogError("_playerTr = null 확인 필요");
        }        
    }
    #endregion
}