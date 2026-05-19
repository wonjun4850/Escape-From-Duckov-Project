using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어 SO")]
    [SerializeField] private PlayerDataSO _defaultPlayerData;

    [Header("플레이어 정보")]
    public string PlayerId;
    public string PlayerName;

    [Header("성장")]
    public int Level;
    public int CurrentExp;
    public int[] MaxExpTable;
    
    [Header("체력")]
    public float MaxHp;

    [Header("이동")]
    public float BaseMoveSpeed;
    public float RunMultiplier;

    [Header("구르기 설정")]
    public float DodgeForce;
    public float DodgeDuration;

    [Header("인벤토리")]
    public int BaseInventorySlots;
    public float BaseCarryWeight;

    [Header("펫 인벤토리")]
    public int PetInventorySlots;

    [Header("스테미너")]
    public float MaxStamina;
    public float StaminaRegenRate;
    public float DodgeCost;
    public float RunCost;

    [Header("생존")]
    public float MaxEnergy;
    public float MaxHydration;
    public float EnergyLossRate;
    public float HydrationLossRate;

    [Header("스태쉬 크기")]
    public int StashSlots;

    [Header("현재값")]
    public float CurrentHp;
    public float CurrentEnergy;
    public float CurrentHydration;
    public int CurrentMoney;
    #endregion

    #region 내부 변수
    public static DataManager Instance { get; private set; }

    public int MaxExp
    {
        get
        {
            int index = Mathf.Clamp(Level - 1, 0, MaxExpTable.Length - 1);
            return MaxExpTable[index];
        }
    }
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        InitFromSO(_defaultPlayerData);
    }

    #region 외부 호출 함수
    public void InitFromSO(PlayerDataSO data)
    {
        PlayerId = data.PlayerId;
        PlayerName = data.PlayerName;
        Level = data.Level;
        CurrentExp = data.CurrentExp;
        MaxExpTable = data.MaxExpTable;
        MaxHp = data.BaseMaxHealth;
        BaseMoveSpeed = data.BaseMoveSpeed;
        RunMultiplier = data.RunMultiplier;
        DodgeForce = data.DodgeForce;
        DodgeDuration = data.DodgeDuration;
        BaseInventorySlots = data.BaseInventorySlots;
        BaseCarryWeight = data.BaseCarryWeight;
        PetInventorySlots = data.PetInventorySlots;
        MaxStamina = data.MaxStamina;
        StaminaRegenRate = data.StaminaRegenRate;
        DodgeCost = data.DodgeCost;
        RunCost = data.RunCost;
        MaxEnergy = data.MaxEnergy;
        MaxHydration = data.MaxHydration;
        EnergyLossRate = data.EnergyLossRate;
        HydrationLossRate = data.HydrationLossRate;
        StashSlots = data.StashSlots;

        CurrentHp = data.BaseMaxHealth;
        CurrentEnergy = data.MaxEnergy;
        CurrentHydration = data.MaxHydration;        
    }
    #endregion
}