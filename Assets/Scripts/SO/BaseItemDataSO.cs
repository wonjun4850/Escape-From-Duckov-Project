using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseItemDataSO : ScriptableObject
{
    public enum EType
    {
        None,
        Weapon,
        Armor,
        Totem,
        Consumable,
        Material,
        Ammo
    }

    #region 인스펙터
    [Header("아이템 정보")]
    [SerializeField] private string _itemId = "";
    [SerializeField] private string _itemName = "";
    [SerializeField] private EType _itemType = EType.None;
    [SerializeField, TextArea] private string _itemDescription = "";
    [SerializeField] private int _itemPrice = 0;
    [SerializeField] private float _itemWeight = 0f;
    [SerializeField] private int _maxStackSize = 1;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private GameObject _itemPrefab;
    #endregion

    #region 프로퍼티
    public string ItemId => _itemId;
    public string ItemName => _itemName;
    public EType ItemType => _itemType;
    public string ItemDescription => _itemDescription;
    public int ItemPrice => _itemPrice;
    public float ItemWeight => _itemWeight;
    public int MaxStackSize => _maxStackSize;
    public Sprite ItemIcon => _itemIcon;
    public GameObject ItemPrefab => _itemPrefab;
    #endregion
}