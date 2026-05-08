using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/ConsumableItemDataSO", fileName = "ConsumableItemDataSO")]
public class ConsumableItemDataSO : BaseItemDataSO
{
    #region 인스펙터
    [Header("소모품 효과 설정 (0이면 효과 없음)")]
    [SerializeField] private float _healthRestore = 0f;
    [SerializeField] private float _energyRestore = 0f;
    [SerializeField] private float _hydrationRestore = 0f;

    [Header("사용 시간")]
    [SerializeField] private float _useTime = 0f;

    [Header("여러번 나눠 쓸 수 있는지? (구급상자, 물통)")]
    [SerializeField] private float _reusableCount = 0f;
    #endregion

    #region 프로퍼티
    public float HealthRestore => _healthRestore;
    public float EnergyRestore => _energyRestore;
    public float HydrationRestore => _hydrationRestore;
    public float UseTime => _useTime;
    public float ReusableCount => _reusableCount;
    #endregion
}
