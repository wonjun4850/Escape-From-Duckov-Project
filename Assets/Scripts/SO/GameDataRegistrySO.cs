using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/GameDataRegistrySO", fileName = "GameDataRegistrySO")]
public class GameDataRegistrySO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private List<PlayerDataSO> _player = new List<PlayerDataSO>();
    [SerializeField] private List<EnemyDataSO> _enemy = new List<EnemyDataSO>();
    [SerializeField] private List<ConsumableItemDataSO> _consumableItem = new List<ConsumableItemDataSO>();
    [SerializeField] private List<MaterialItemDataSO> _materialItem = new List<MaterialItemDataSO>();
    [SerializeField] private List<AmmoItemDataSO> _ammoItem = new List<AmmoItemDataSO>();
    [SerializeField] private List<WeaponItemDataSO> _weaponItem = new List<WeaponItemDataSO>();
    [SerializeField] private List<ArmorItemDataSO> _armorItem = new List<ArmorItemDataSO>();
    [SerializeField] private List<TotemItemDataSO> _totemItem = new List<TotemItemDataSO>();
    #endregion

    #region 프로퍼티
    public IReadOnlyList<PlayerDataSO> Player => _player;
    public IReadOnlyList<EnemyDataSO> Enemy => _enemy;
    public IReadOnlyList<ConsumableItemDataSO> ConsumableItem => _consumableItem;
    public IReadOnlyList<MaterialItemDataSO> MaterialItem => _materialItem;
    public IReadOnlyList<AmmoItemDataSO> AmmoItem => _ammoItem;
    public IReadOnlyList<WeaponItemDataSO> WeaponItem => _weaponItem;
    public IReadOnlyList<ArmorItemDataSO> ArmorItem => _armorItem;
    public IReadOnlyList<TotemItemDataSO> TotemItem => _totemItem;
    #endregion
}