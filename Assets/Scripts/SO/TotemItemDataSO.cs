using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/TotemItemDataSO", fileName = "TotemItemDataSO")]
public class TotemItemDataSO : EquipmentItemDataSO
{
    public enum ETotemType
    {
        None,
        Attack,
        Defense,
        Health,
        CarryWeight,
        InventorySlot,
    }

    #region 인스펙터
    [Header("토템 설정")]
    [SerializeField] private ETotemType _totemType = ETotemType.None;
    [SerializeField] private int _totemLevel = 1;

    [Header("토템 효과 (수치만)")]
    [SerializeField] private int _bonusValue = 0;
    #endregion

    #region 프로퍼티
    public ETotemType TotemType => _totemType;
    public int TotemLevel => _totemLevel;
    public int BonusValue => _bonusValue;
    #endregion
}
