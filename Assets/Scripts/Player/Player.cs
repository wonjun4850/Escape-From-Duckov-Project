using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 인스펙터
    [Header("SO 연결")]
    [SerializeField] private PlayerDataSO _playerData;

    [Header("사망 시 생성할 오브젝트")]
    [SerializeField] private GameObject _deadPrefab;
    #endregion

    #region 내부 변수
    private PlayerMovement _playerMovement;
    private StaminaSystem _staminaSystem;
    private SurvivalSystem _survivalSystem;
    private HpSystem _hpSystem;
    private PlayerAnimation _playerAnimation;
    private Weapon _currentWeapon;
    #endregion

    #region 프로퍼티
    public PlayerDataSO PlayerData => _playerData;
    public PlayerMovement Movement => _playerMovement;
    public StaminaSystem Stamina => _staminaSystem;
    public SurvivalSystem Survival => _survivalSystem;
    public HpSystem Hp => _hpSystem;
    public PlayerAnimation Animation => _playerAnimation;
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _staminaSystem = GetComponent<StaminaSystem>();
        _survivalSystem = GetComponent<SurvivalSystem>();
        _hpSystem = GetComponent<HpSystem>();
        _playerAnimation = GetComponent<PlayerAnimation>();

        if (_playerMovement == null || _staminaSystem == null || _survivalSystem == null || _hpSystem == null || _playerAnimation == null)
        {
            Debug.LogError("Player 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }
    }

    private void Start()
    {
        SetupWeapon();
    }

    private void OnEnable()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnDead += HandleDead;
        }
    }

    private void OnDisable()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnDead -= HandleDead;
        }

        if (_currentWeapon != null)
        {
            _currentWeapon.OnWeaponFire -= HandleWeaponFire;
        }
    }

    private void HandleWeaponFire(WeaponItemDataSO.EWeaponType weaponType)
    {
        if (weaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            _playerAnimation.Attack();
        }
    }

    private void HandleDead()
    {
        var listener = GetComponent<AudioListener>();

        if (listener != null)
        {
            Destroy(listener);
        }

        if (_deadPrefab != null)
        {
            Instantiate(_deadPrefab, transform.position, transform.rotation);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDead();
        }

        Destroy(gameObject);
    }

    #region 외부 호출 함수
    public void Init()
    {
        var data = DataManager.Instance;

        if (data == null)
        {
            Debug.LogError("DataManager.Instance == null");
            return;
        }

        _playerMovement.Init(data.BaseMoveSpeed, data.RunMultiplier, data.DodgeForce, data.DodgeDuration);
        _staminaSystem.Init(data.MaxStamina, data.StaminaRegenRate, data.DodgeCost, data.RunCost);
        _survivalSystem.Init(data.MaxEnergy, data.MaxHydration, data.EnergyLossRate, data.HydrationLossRate, data.CurrentEnergy, data.CurrentHydration);
        _hpSystem.Init(data.MaxHp, data.CurrentHp);
    }

    public void SetupWeapon()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnWeaponFire -= HandleWeaponFire;
        }

        _currentWeapon = GetComponentInChildren<Weapon>();

        if (_currentWeapon != null)
        {
            _currentWeapon.OnWeaponFire += HandleWeaponFire;
        }
    }
    #endregion
}