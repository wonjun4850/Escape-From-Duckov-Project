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
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _staminaSystem = GetComponent<StaminaSystem>();
        _survivalSystem = GetComponent<SurvivalSystem>();

        if (_playerMovement == null || _staminaSystem == null)
        {
            Debug.LogError("Player 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }        

        if (_playerData != null)
        {
            Init(_playerData);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region 외부 호출 함수
    public void Init(PlayerDataSO data)
    {
        _playerData = data;

        if (_playerMovement != null)
        {
            _playerMovement.Init(_playerData.BaseMoveSpeed, _playerData.RunMultiplier, _playerData.DodgeForce, _playerData.DodgeDuration);
        }

        if (_staminaSystem != null)
        {
            _staminaSystem.Init(_playerData.MaxStamina, _playerData.StaminaRegenRate, _playerData.DodgeCost, _playerData.RunCost);
        }

        if (_survivalSystem != null)
        {
            _survivalSystem.Init(_playerData.MaxEnergy, _playerData.MaxHydration, _playerData.EnergyLossRate, _playerData.HydrationLossRate);
        }

        Debug.Log("플레이어 데이터 주입 완료");
    }
    #endregion
}