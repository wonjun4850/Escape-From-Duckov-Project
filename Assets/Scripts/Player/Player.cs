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
    public PlayerDataSO PlayerData => _playerData;
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
    #endregion
}