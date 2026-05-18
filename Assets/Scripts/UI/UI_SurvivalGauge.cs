using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SurvivalGauge : MonoBehaviour
{
    public enum EType
    {
        Hydration,
        Energy
    }

    #region 인스펙터
    [Header("설정")]
    [SerializeField] private EType _type;
    [SerializeField] private Image _backgroundGauge;
    [SerializeField] private Image _fillGauge;

    [Header("색상 설정 (배경)")]
    [SerializeField] private Image _background;
    [SerializeField] private Color _normalColor = new Color();
    [SerializeField] private Color _warningColor = new Color();
    #endregion

    #region 내부 변수
    private SurvivalSystem _survivalSystem;
    #endregion

    private void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (_survivalSystem != null)
        {
            if (_type == EType.Hydration)
            {
                _survivalSystem.OnHydrationChanged -= RefreshUI;
            }

            else
            {
                _survivalSystem.OnEnergyChanged -= RefreshUI;
            }
        }
    }

    private void RefreshUI(float ratio)
    {
        _backgroundGauge.fillAmount = ratio;
        _fillGauge.fillAmount = ratio;

        if (ratio <= 0f)
        {
            _background.color = _warningColor;
        }

        else
        {
            _background.color = _normalColor;
        }
    }

    #region 외부 호출 함수
    public void SetUp(SurvivalSystem survival)
    {
        UnBind();

        _survivalSystem = survival;

        if (_survivalSystem != null)
        {
            if (_type == EType.Hydration)
            {
                _survivalSystem.OnHydrationChanged += RefreshUI;
            }

            else
            {
                _survivalSystem.OnEnergyChanged += RefreshUI;
            }

            float initRatio = (_type == EType.Hydration) ? _survivalSystem.GetHydrationRatio() : _survivalSystem.GetEnergyRatio();

            RefreshUI(initRatio);
        }

        else
        {
            Debug.LogError("_survivalSystem = null 확인 필요");
        }
    }
    #endregion
}