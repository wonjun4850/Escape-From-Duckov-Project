using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private EnemyDataSO _enemyData;
    #endregion

    #region 내부 변수
    private HpSystem _hpSystem;
    #endregion

    #region 프로퍼티
    public EnemyDataSO EnemyData => _enemyData;
    public HpSystem Hp => _hpSystem;
    #endregion

    private void Awake()
    {
        _hpSystem = GetComponent<HpSystem>();

        if (_hpSystem == null)
        {
            Debug.LogError("Enemy 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }
    }

    #region 외부 호출 함수
    public void Init(EnemyDataSO data)
    {
        _enemyData = data;

        if (_hpSystem != null)
        {
            _hpSystem.Init(_enemyData.BaseMaxHealth);
            Debug.Log("적 데이터 (_hpSystem) 주입 완료");
        }

        else
        {
            Debug.Log("적 데이터 (_hpSystem) 주입 실패");
        }
    }
    #endregion
}