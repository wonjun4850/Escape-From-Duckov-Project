using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItemDataSO : BaseItemDataSO
{
    public enum EEquipmentSlot
    {
        None,
        Weapon,
        MeleeWeapon,
        Head,
        Body,
        Backpack,
        Totem
    }

    #region 인스펙터
    [Header("장비 슬롯 설정")]
    [SerializeField] private EEquipmentSlot _equipmentSlot = EEquipmentSlot.None;
    #endregion

    #region 프로퍼티
    public EEquipmentSlot EquipmentSlot => _equipmentSlot;
    #endregion
}
