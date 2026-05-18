using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpGauge : MonoBehaviour
{
    #region 인스펙터
    [Header("숫자 텍스트 (현재체력 / 최대체력)")]
    [SerializeField] private TextMeshProUGUI _currentHpText;
    [SerializeField] private TextMeshProUGUI _maxHpText;

    [Header("게이지 Mask 연결")]
    [SerializeField] private RectMask2D _hpMask;

    [Header("셰이더 적용된 백그라운드")]
    [SerializeField] private Image _backgroundHpBarShader;
    [SerializeField] private string _shaderRef = "_ClipAmount";

    [Header("hp바 백그라운드")]
    [SerializeField] private Image _backgroundHpBar;
    [SerializeField] private Color _deadColor = new Color();
    #endregion

    #region 내부 변수
    private HpSystem _hpSystem;
    private Material _shaderMat;
    #endregion

    private void Awake()
    {
        if (_backgroundHpBarShader != null)
        {
            _shaderMat = _backgroundHpBarShader.material;
        }
    }

    private void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnHpChanged -= RefreshUI;
        }
    }

    private void RefreshUI(float ratio)
    {
        _currentHpText.text = _hpSystem.GetCurrentHp().ToString("F1");
        _maxHpText.text = _hpSystem.GetMaxHp().ToString("F1");

        float width = _hpMask.rectTransform.rect.width;
        float padding = width * (1f - ratio);
        _hpMask.padding = new Vector4(0, 0, padding, 0);

        float clipValue = 1f - ratio;
        _shaderMat.SetFloat(_shaderRef, clipValue);

        if (ratio <= 0)
        {
            _backgroundHpBar.color = _deadColor;
        }
    }

    #region 외부 호출 함수
    public void Setup(HpSystem hp)
    {
        UnBind();

        _hpSystem = hp;

        if (_hpSystem != null)
        {
            _hpSystem.OnHpChanged += RefreshUI;

            RefreshUI(_hpSystem.GetHpRatio());
        }

        else
        {
            Debug.LogError("_hpSystem = null 확인 필요");
        }
    }
    #endregion
}