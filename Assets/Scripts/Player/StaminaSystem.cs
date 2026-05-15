using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    #region 내부 변수
    private float _maxStamina;
    private float _staminaRegenRate;
    private float _dodgeCost;
    private float _runCost;

    private float _currentStamina;
    private float _regenDelay = 1f;
    private float _currentRegenTimer = 0f;

    private bool _isInit = false;
    #endregion

    void Update()
    {
        if (!_isInit)
        {
            return;
        }

        if (_currentRegenTimer > 0)
        {
            _currentRegenTimer -= Time.deltaTime;
        }

        else
        {
            StaminaRegen();
        }
    }

    private void StaminaRegen()
    {
        if (_currentStamina <= _maxStamina)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
        }
    }

    private void ResetStaminaRegenTimer()
    {
        _currentRegenTimer = _regenDelay;
    }

    #region 외부 호출 함수
    public void Init(float maxStamina, float staminaRegenRate, float dodgeCost, float runCost)
    {
        _maxStamina = maxStamina;
        _staminaRegenRate = staminaRegenRate;
        _dodgeCost = dodgeCost;
        _runCost = runCost;

        _currentStamina = maxStamina;

        _isInit = true;
    }

    public void ConsumeStaminaByRun()
    {
        if (!_isInit || !CanRun())
        {
            return;
        }

        _currentStamina -= _runCost * Time.fixedDeltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);

        ResetStaminaRegenTimer();
    }

    public bool CanRun()
    {
        return _currentStamina > 0;
    }

    public bool CanRunStart()
    {
        return _currentStamina >= (_maxStamina * 0.2f);
    }

    public void ConsumeStaminaByDodge()
    {
        if (!CanDodge())
        {
            return;
        }

        _currentStamina -= _dodgeCost;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);

        ResetStaminaRegenTimer();
    }

    public bool CanDodge()
    {
        return _currentStamina >= _dodgeCost;
    }

    public bool IsStaminaLow()
    {
        return (_currentStamina / _maxStamina) < 0.2f;
    }

    // UI 에서 사용할 함수
    public float GetStaminaRatio()
    {
        return _currentStamina / _maxStamina;
    }
    #endregion
}