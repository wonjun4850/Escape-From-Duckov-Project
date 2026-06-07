using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private EnemyDataSO _enemyData;
    #endregion

    #region 내부 변수
    private HpSystem _hpSystem;
    private Weapon _currentWeapon;
    private Transform _target;
    private EnemyAnimation _enemyAnimation;
    #endregion

    #region 프로퍼티
    public EnemyDataSO EnemyData => _enemyData;
    public HpSystem Hp => _hpSystem;
    public Weapon CurrentWeapon => _currentWeapon;
    #endregion

    private void Awake()
    {
        _hpSystem = GetComponent<HpSystem>();
        //_target = GetComponent<Transform>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _currentWeapon = GetComponentInChildren<Weapon>();

        if (_hpSystem == null || _enemyData == null || _enemyAnimation == null)
        {
            Debug.LogError("Enemy 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }
    }

    private void OnEnable()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnDead += HandleDead;
        }

        if (_currentWeapon != null)
        {
            _currentWeapon.OnWeaponFire += HandleWeaponFire;
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

    private void Start()
    {
        // 테스트용
        if (_currentWeapon != null)
        {
            _currentWeapon.Init(_currentWeapon.WeaponData, this.gameObject);
        }
    }

    private void HandleWeaponFire(WeaponItemDataSO.EWeaponType weaponType)
    {
        if (weaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            _enemyAnimation.Attack();
        }
    }

    private void HandleDead()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKillExp(_enemyData.ExpReward);
        }

        if (_enemyData.ChickenPrefab != null)
        {
            GameObject chicken = Instantiate(_enemyData.ChickenPrefab, transform.position, transform.rotation);
            // 아이템을 넣어줘야한다면 이곳에서 처리하자
        }

        Destroy(gameObject);
    }

    #region 외부 호출 함수
    public void Init(EnemyDataSO data)
    {
        _enemyData = data;

        if (_hpSystem != null)
        {
            _hpSystem.Init(_enemyData.BaseMaxHealth, _enemyData.BaseMaxHealth);
            Debug.Log("적 데이터 (_hpSystem) 주입 완료");
        }
    }
    #endregion
}