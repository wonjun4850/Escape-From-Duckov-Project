using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    [System.Flags]
    public enum ESurvivalState
    {
        None = 0,
        Hunger = 1 << 0,
        Thirst = 1 << 1
    }

    #region 내부 변수
    private ESurvivalState _currentState = ESurvivalState.None;

    private float _maxEnergy;
    private float _maxHydration;
    private float _energyLossRate;
    private float _hydrationLossRate;

    private float _currentEnergy;
    private float _currentHydration;

    private bool _isInit = false;
    #endregion

    #region 이벤트
    public event System.Action<ESurvivalState> OnStateChanged;
    public event System.Action<float> OnEnergyChanged;
    public event System.Action<float> OnHydrationChanged;
    #endregion

    void Update()
    {
        if (!_isInit)
        {
            return;
        }

        ConsumeByTime();
        SetSurvivalState(DecideSurvivalState());
    }

    private void ConsumeByTime()
    {
        _currentEnergy -= _energyLossRate * Time.deltaTime;
        _currentHydration -= _hydrationLossRate * Time.deltaTime;

        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        _currentHydration = Mathf.Clamp(_currentHydration, 0, _maxHydration);

        OnEnergyChanged?.Invoke(GetEnergyRatio());
        OnHydrationChanged?.Invoke(GetHydrationRatio());
    }

    private void SetSurvivalState(ESurvivalState next)
    {
        if (_currentState == next)
        {
            return;
        }

        _currentState = next;

        OnStateChanged?.Invoke(_currentState);
    }

    private ESurvivalState DecideSurvivalState()
    {
        ESurvivalState nextState = ESurvivalState.None;

        if (_currentEnergy <= 0)
        {
            nextState |= ESurvivalState.Hunger;
        }

        if (_currentHydration <= 0)
        {
            nextState |= ESurvivalState.Thirst;
        }

        return nextState;
    }

    #region 외부 호출 함수
    public void Init(float maxEnergy, float maxHydration, float energyLossRate, float hydrationLossRate, float currentEnergy, float currentHydration)
    {
        _maxEnergy = maxEnergy;
        _maxHydration = maxHydration;
        _energyLossRate = energyLossRate;
        _hydrationLossRate = hydrationLossRate;

        _currentEnergy = currentEnergy;
        _currentHydration = currentHydration;

        _isInit = true;
    }

    // 음식 사용했을때 사용할 함수
    public void RestoreEnergy(float amount)
    {
        _currentEnergy += amount;
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        OnEnergyChanged?.Invoke(GetEnergyRatio());
    }

    // 음식 사용했을때 사용할 함수
    public void RestoreHydration(float amount)
    {
        _currentHydration += amount;
        _currentHydration = Mathf.Clamp(_currentHydration, 0, _maxHydration);
        OnHydrationChanged?.Invoke(GetHydrationRatio());
    }

    // UI 에서 사용할 함수
    public float GetEnergyRatio()
    {
        return _currentEnergy / _maxEnergy;
    }

    // UI 에서 사용할 함수
    public float GetHydrationRatio()
    {
        return _currentHydration / _maxHydration;
    }

    public float GetCurrentEnergy()
    {
        return _currentEnergy;
    }

    public float GetCurrentHydration()
    {
        return _currentHydration;
    }

    public float GetMaxEnergy()
    {
        return _maxEnergy;
    }

    public float GetMaxHydration()
    {
        return _maxHydration;
    }

    // 상태 반환 함수
    public ESurvivalState GetState()
    {
        return _currentState;
    }
    #endregion
}