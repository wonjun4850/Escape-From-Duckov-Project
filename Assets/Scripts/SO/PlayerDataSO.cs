using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/PlayerDataSO", fileName = "PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    #region 인스펙터
    [Header("플레이어 정보")]
    [SerializeField] private string _playerId = "Player_01";
    [SerializeField] private string _playerName = "Player";

    [Header("성장")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp = 0;
    [SerializeField] private int[] _maxExp = new int[] { 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000, 10500, 11000, 999999999 };

    [Header("체력")]
    [SerializeField] private float _baseMaxHealth = 40;

    [Header("이동")]
    [SerializeField] private float _baseMoveSpeed = 5f;
    [SerializeField] private float _runMultiplier = 1.5f;

    [Header("구르기 설정")]
    [SerializeField] private float _dodgeForce = 10f;
    [SerializeField] private float _dodgeDuration = 0.5f;

    [Header("인벤토리")]
    [SerializeField] private int _baseInventorySlots = 10;
    [SerializeField] private float _baseCarryWeight = 45f;

    [Header("펫 인벤토리")]
    [SerializeField] private int _petInventorySlots = 1;

    [Header("스테미너")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaRegenRate = 20f;
    [SerializeField] private float _dodgeCost = 10f;
    [SerializeField] private float _runCost = 7f;

    [Header("생존")]
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _maxHydration = 100f;
    [SerializeField] private float _energyLossRate = 0.1f;
    [SerializeField] private float _hydrationLossRate = 0.1f;

    [Header("프리팹")]
    [SerializeField] private GameObject _playerPrefab;
    #endregion

    #region 프로퍼티
    public string PlayerId => _playerId;
    public string PlayerName => _playerName;
    public int Level => _level;
    public int CurrentExp => _currentExp;
    public int MaxExp
    {
        get
        {
            int index = Mathf.Clamp(_level - 1, 0, _maxExp.Length - 1);
            return _maxExp[index];
        }
    }
    public float BaseMaxHealth => _baseMaxHealth;
    public float BaseMoveSpeed => _baseMoveSpeed;
    public float RunMultiplier => _runMultiplier;
    public float DodgeForce => _dodgeForce;
    public float DodgeDuration => _dodgeDuration;
    public int BaseInventorySlots => _baseInventorySlots;
    public float BaseCarryWeight => _baseCarryWeight;
    public int PetInventorySlots => _petInventorySlots;
    public float MaxStamina => _maxStamina;
    public float StaminaRegenRate => _staminaRegenRate;
    public float DodgeCost => _dodgeCost;
    public float RunCost => _runCost;
    public float MaxEnergy => _maxEnergy;
    public float MaxHydration => _maxHydration;
    public float EnergyLossRate => _energyLossRate;
    public float HydrationLossRate => _hydrationLossRate;
    public GameObject PlayerPrefab => _playerPrefab;
    #endregion

    #region 외부 호출 함수
    public void LevelUp()
    {
        _level++;
    }

    public void AddExp(int remainExp)
    {
        _currentExp += remainExp;
    }

    public int GetMaxExpForLevel(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, _maxExp.Length - 1);
        return _maxExp[index];
    }
    #endregion
}