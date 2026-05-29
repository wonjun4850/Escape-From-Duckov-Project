using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private EnemyDataSO _enemyData;

    [Header("패트롤 범위 씬뷰 시각화 (테스트용도)")]
    [SerializeField] private float _patrolRange = 10f;
    #endregion

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (_enemyData == null || _enemyData.EnemyPrefab == null)
        {
            Debug.LogError("EnemySpawner null 확인 필요");
            return;
        }

        GameObject enemyObj = Instantiate(_enemyData.EnemyPrefab, transform.position, transform.rotation);

        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Init(_enemyData);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _patrolRange);
    }
}