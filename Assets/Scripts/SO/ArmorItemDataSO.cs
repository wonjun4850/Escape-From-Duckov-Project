using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/ArmorItemDataSO", fileName = "ArmorItemDataSO")]
public class ArmorItemDataSO : EquipmentItemDataSO
{
    #region 인스펙터
    [Header("방어구/가방 설정")]
    [SerializeField] private float _armorValue = 0f;
    [SerializeField] private int _extraInventorySlots = 0;
    [SerializeField] private float _extraCarryWeight = 0f;
    #endregion

    #region 프로퍼티
    public float ArmorValue => _armorValue;
    public int ExtraInventorySlots => _extraInventorySlots;
    public float ExtraCarryWeight => _extraCarryWeight;
    #endregion
}