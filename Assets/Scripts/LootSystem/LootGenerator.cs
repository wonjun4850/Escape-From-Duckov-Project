using System.Collections.Generic;
using UnityEngine;

public static class LootGenerator
{
    public static bool TryGenerateRandomLoot(LootTableSO lootTable, out GeneratedLoot generatedLoot)
    {
        generatedLoot = default;

        if (lootTable == null)
        {
            return false;
        }

        var lootItems = lootTable.RandomLootItems;

        float totalWeight = 0f;

        foreach (var lootItem in lootItems)
        {
            if (lootItem.itemData == null || lootItem.weight <= 0f)
            {
                continue;
            }

            if (lootItem.minAmount < 1 || lootItem.maxAmount < 1)
            {
                continue;
            }

            totalWeight += lootItem.weight;
        }

        if (totalWeight <= 0f)
        {
            return false;
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (var lootItem in lootItems)
        {
            if (lootItem.itemData == null || lootItem.weight <= 0f)
            {
                continue;
            }

            if (lootItem.minAmount < 1 || lootItem.maxAmount < 1)
            {
                continue;
            }

            randomValue -= lootItem.weight;

            if (randomValue < 0f)
            {
                int min = Mathf.Min(lootItem.minAmount, lootItem.maxAmount);
                int max = Mathf.Max(lootItem.minAmount, lootItem.maxAmount);

                generatedLoot.itemData = lootItem.itemData;
                generatedLoot.amount = Random.Range(min, max + 1);
                generatedLoot.searchState = ELootSearchState.Unsearched;

                return true;
            }
        }

        return false;
    }

    public static bool TryGenerateFixedLoot(LootTableSO lootTable, out List<GeneratedLoot> generatedLoot)
    {
        generatedLoot = new List<GeneratedLoot>();

        if (lootTable == null)
        {
            return false;
        }

        var lootItems = lootTable.FixedLootItems;

        foreach (var lootItem in lootItems)
        {
            if (lootItem.itemData == null)
            {
                continue;
            }

            if (lootItem.minAmount < 1 || lootItem.maxAmount < 1)
            {
                continue;
            }

            int min = Mathf.Min(lootItem.maxAmount, lootItem.minAmount);
            int max = Mathf.Max(lootItem.maxAmount, lootItem.minAmount);
            int randomAmount = Random.Range(min, max + 1);

            generatedLoot.Add(new GeneratedLoot
            {
                itemData = lootItem.itemData,
                amount = randomAmount,
                searchState = ELootSearchState.Unsearched
            });
        }

        if (generatedLoot.Count <= 0)
        {
            return false;
        }

        return true;
    }
}