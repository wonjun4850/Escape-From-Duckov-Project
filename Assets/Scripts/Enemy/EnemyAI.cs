using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EEnemyState
    {
        Patrol,
        Chase,
        Attack,
    }

    #region 인스펙터
    [Header("감지 범위")]
    [SerializeField] private float _detectionRadius = 20f;

    [Header("공격 상태 설정")]
    [SerializeField] private float _attackRadius = 10f;
    [SerializeField] private float _rotSpeed = 10f;

    [Header("패트롤 범위")]
    [SerializeField] private float _patrolRange = 10f;
    #endregion

    #region 내부 변수
    private FSMState _currentState;
    private Dictionary<EEnemyState, FSMState> _states = new Dictionary<EEnemyState, FSMState>();
    #endregion

    #region 프로퍼티
    public Enemy Enemy { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform PlayerTr { get; private set; }
    public float DetectionRadius => _detectionRadius;
    public float AttackRadius => _attackRadius;
    public float RotSpeed => _rotSpeed;
    public float PatrolRange => _patrolRange;
    public Vector3 SpawnPosition { get; private set; }
    #endregion


    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        Agent = GetComponent<NavMeshAgent>();

        _states.Add(EEnemyState.Patrol, new PatrolState(this));
        _states.Add(EEnemyState.Chase, new ChaseState(this));
        _states.Add(EEnemyState.Attack, new AttackState(this));
    }

    private void Start()
    {
        SpawnPosition = transform.position;

        StartCoroutine(CoBindPlayer());
    }

    private IEnumerator CoBindPlayer()
    {
        GameObject player = null;

        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        PlayerTr = player.transform;

        ChangeState(EEnemyState.Patrol);
    }

    private void Update()
    {
        if (PlayerTr == null)
        {
            return;
        }

        if (_currentState != null)
        {
            _currentState.OnStateUpdate();
        }
    }

    public void ChangeState(EEnemyState newState)
    {
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }

        _currentState = _states[newState];

        _currentState.OnStateEnter();
    }
}