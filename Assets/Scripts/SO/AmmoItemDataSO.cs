using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/AmmoItemDataSO", fileName = "AmmoItemDataSO")]
public class AmmoItemDataSO : BaseItemDataSO
{
    public enum EAmmoType
    {
        None,
        Pistol,
        Rifle,
        Shotgun,
        BattleRifle
    }

    #region 인스펙터
    [Header("탄약 타입 설정")]
    [SerializeField] private EAmmoType _ammoType = EAmmoType.None;
    #endregion

    #region 프로퍼티
    public EAmmoType AmmoType => _ammoType;
    #endregion
}