
[System.Serializable]
public struct GeneratedLoot
{
    public BaseItemDataSO itemData;
    public int amount;
    public ELootSearchState searchState;
}

public enum ELootSearchState
{
    Unsearched,
    Searching,
    Searched,
    // 추후 아이템 획득 처리할때 추가해야함
}