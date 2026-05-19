using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private WeaponItemDataSO _weaponData;
    #endregion

    #region 내부 변수
    public GameObject Owner { get; private set; }
    #endregion

    #region 프로퍼티
    public WeaponItemDataSO WeaponData => _weaponData;    
    #endregion

    private void Awake()
    {
        if (_weaponData == null)
        {
            Debug.LogError("Weapon 확인 필요");
        }
    }

    #region 외부 호출 함수
    public void Init(WeaponItemDataSO data, GameObject owner)
    {
        _weaponData = data;
        Owner = owner;

        if (!owner.CompareTag("Player"))
        {
            float enemySpreadOffset = owner.CompareTag("Boss") ? data.Spread * 1.2f : data.Spread * 2f;
            // 스프레드 적용 함수
        }

    }
    #endregion
}