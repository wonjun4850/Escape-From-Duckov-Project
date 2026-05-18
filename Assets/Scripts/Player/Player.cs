using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 인스펙터
    [Header("SO 연결")]
    [SerializeField] private PlayerDataSO _playerData;
    #endregion

    #region 내부 변수
    private PlayerMovement _playerMovement;
    private StaminaSystem _staminaSystem;
    private SurvivalSystem _survivalSystem;
    private HpSystem _hpSystem;
    #endregion

    #region 프로퍼티
    public PlayerMovement Movement => _playerMovement;
    public StaminaSystem Stamina => _staminaSystem;
    public SurvivalSystem Survival => _survivalSystem;
    public HpSystem Hp => _hpSystem;
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _staminaSystem = GetComponent<StaminaSystem>();
        _survivalSystem = GetComponent<SurvivalSystem>();
        _hpSystem = GetComponent<HpSystem>();

        if (_playerMovement == null || _staminaSystem == null || _survivalSystem == null || _hpSystem == null)
        {
            Debug.LogError("Player 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }        
    }

    void Start()
    {
        if (_playerData != null)
        {
            Init(_playerData);
        }
    }

    #region 외부 호출 함수
    public void Init(PlayerDataSO data)
    {
        _playerData = data;

        if (_playerMovement != null)
        {
            _playerMovement.Init(_playerData.BaseMoveSpeed, _playerData.RunMultiplier, _playerData.DodgeForce, _playerData.DodgeDuration);
            Debug.Log("플레이어 데이터 (_playerMovement) 주입 완료");
        }

        else
        {
            Debug.Log("플레이어 데이터 (_playerMovement) 주입 실패");
        }

        if (_staminaSystem != null)
        {
            _staminaSystem.Init(_playerData.MaxStamina, _playerData.StaminaRegenRate, _playerData.DodgeCost, _playerData.RunCost);
            Debug.Log("플레이어 데이터 (_staminaSystem) 주입 완료");
        }

        else
        {
            Debug.Log("플레이어 데이터 (_staminaSystem) 주입 실패");
        }

        if (_survivalSystem != null)
        {
            _survivalSystem.Init(_playerData.MaxEnergy, _playerData.MaxHydration, _playerData.EnergyLossRate, _playerData.HydrationLossRate);
            Debug.Log("플레이어 데이터 (_survivalSystem) 주입 완료");
        }

        else
        {
            Debug.Log("플레이어 데이터 (_survivalSystem) 주입 실패");
        }

        if (_hpSystem != null)
        {
            _hpSystem.Init(_playerData.BaseMaxHealth);
            Debug.Log("플레이어 데이터 (_hpSystem) 주입 완료");
        }

        else
        {
            Debug.Log("플레이어 데이터 (_hpSystem) 주입 실패");
        }

        if (IngameUIManager.Instance != null)
        {
            IngameUIManager.Instance.BindPlayerUI(this);
        }

        else
        {
            Debug.Log("플레이어 데이터 -> UI 데이터 주입 실패");
        }
    }
    #endregion
}