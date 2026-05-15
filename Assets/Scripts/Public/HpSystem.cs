using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour, IDamageable
{
    #region 인스펙터
    [Header("플레이어일시 체크")]
    [SerializeField] private bool _isPlayer = false;
    #endregion

    #region 내부 변수
    private SurvivalSystem _survivalSystem;

    private float _baseMaxHealth;

    private float _currentHP;
    private float _hungerTickTimer;

    private bool _isInit = false;
    private bool _isDead = false;
    #endregion

    #region 프로퍼티
    public bool IsDead => _isDead;
    #endregion

    private void Awake()
    {
        _survivalSystem = GetComponent<SurvivalSystem>();
    }

    void Update()
    {
        if (!_isInit || _isDead)
        {
            return;
        }

        if (_isPlayer)
        {
            CheckHungerDamage();
        }
    }    

    private void CheckHungerDamage()
    {
        if (_survivalSystem.GetState().HasFlag(SurvivalSystem.ESurvivalState.Hunger))
        {
            _hungerTickTimer += Time.deltaTime;

            if (_hungerTickTimer >= 3f)
            {
                TakeDamage(1.0f);
                _hungerTickTimer = 0f;
            }

            else
            {
                _hungerTickTimer = 0f;
            }
        }
    }

    #region 외부 호출 함수
    public void Init(float baseMaxHealth)
    {
        _baseMaxHealth = baseMaxHealth;
        _currentHP = baseMaxHealth;
        _isInit = true;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead)
        {
            return;
        }

        _currentHP -= amount;

        if (_currentHP <= 0)
        {
            _currentHP = 0;
            _isDead = true;
        }
    }

    public void Heal(float amount)
    {
        if (_isDead)
        {
            return;
        }

        _currentHP += amount;
        _currentHP = Mathf.Clamp(_currentHP, 0, _baseMaxHealth);
    }

    // UI 에서 사용할 함수
    public float GetHpRatio()
    {
        return _currentHP / _baseMaxHealth;
    }
    #endregion
}