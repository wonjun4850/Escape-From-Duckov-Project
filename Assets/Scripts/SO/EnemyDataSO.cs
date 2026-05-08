using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/EnemyDataSO", fileName = "EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public enum EType
    {
        Base,
        Melee,
        Elite,
        Boss
    }

    #region 인스펙터
    [Header("적 정보")]
    [SerializeField] private string _enemyId = "";
    [SerializeField] private string _enemyName = "";
    [SerializeField] private EType _enemyType = EType.Base;

    [Header("체력")]
    [SerializeField] private float _baseMaxHealth = 40;

    [Header("이동")]
    [SerializeField] private float _baseMoveSpeed = 5f;

    [Header("적 프리팹")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("보스 패턴용")]
    [SerializeField] private bool _canDodge = false;
    [SerializeField] private float _dodgeCooldown = 7f;
    [SerializeField] private float _dodgeForce = 10f;
    [SerializeField] private float _dodgeDuration = 0.5f;

    [Header("보상 및 시체(치킨)프리팹")]
    [SerializeField] private int _expReward = 10;
    [SerializeField] private GameObject _chickenPrefab;
    #endregion

    #region 프로퍼티
    public string EnemyId => _enemyId;
    public string EnemyName => _enemyName;
    public EType EnemyType => _enemyType;
    public float BaseMaxHealth => _baseMaxHealth;
    public float BaseMoveSpeed => _baseMoveSpeed;
    public GameObject EnemyPrefab => _enemyPrefab;
    public bool CanDodge => _canDodge;
    public float DodgeCooldown => _dodgeCooldown;
    public float DodgeForce => _dodgeForce;
    public float DodgeDuration => _dodgeDuration;
    public int ExpReward => _expReward;
    public GameObject ChickenPrefab => _chickenPrefab;
    #endregion
}