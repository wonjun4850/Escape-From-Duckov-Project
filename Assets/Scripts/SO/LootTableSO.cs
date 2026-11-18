using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/LootTable/LootTable_SO", fileName = "LootTable_SO")]
public class LootTableSO : ScriptableObject
{
    [System.Serializable]
    public struct RandomLootItem
    {
        public BaseItemDataSO itemData;
        public float weight;
        public int minAmount;
        public int maxAmount;
    }

    [System.Serializable]
    public struct FixedLootItem
    {
        public BaseItemDataSO itemData;
        public int minAmount;
        public int maxAmount;
    }

    #region 인스펙터
    [SerializeField] private List<RandomLootItem> _randomLootItems = new List<RandomLootItem>();
    [SerializeField] private List<FixedLootItem> _fixedLootItems = new List<FixedLootItem>();
    #endregion

    #region 내부 변수
    public IReadOnlyList<RandomLootItem> RandomLootItems => _randomLootItems;
    public IReadOnlyList<FixedLootItem> FixedLootItems => _fixedLootItems;
    #endregion
}