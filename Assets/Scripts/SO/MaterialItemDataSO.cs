using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/MaterialItemDataSO", fileName = "MaterialItemDataSO")]
public class MaterialItemDataSO : BaseItemDataSO
{
    public enum EMaterialType
    {
        None,
        Farming,
        Corpse,
        All
    }

    #region 인스펙터
    [Header("재료 아이템 타입 설정")]
    [SerializeField] private EMaterialType _materialType = EMaterialType.None;
    #endregion

    #region 프로퍼티
    public EMaterialType MaterialType => _materialType;
    #endregion
}