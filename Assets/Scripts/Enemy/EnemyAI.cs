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
    [Header("사거리 설정")]
    [SerializeField] private float _detectionRadius = 20f;
    [SerializeField] private float _attackRadius = 10f;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerTr = player.transform;
        }

        ChangeState(EEnemyState.Patrol);
    }

    private void Update()
    {
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