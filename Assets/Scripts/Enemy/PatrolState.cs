using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{
    public PatrolState(EnemyAI ai) : base(ai) { }

    #region 내부 변수
    private float _waitTimer;
    private float _waitTime = 2f;
    private bool _isWaiting = false;
    #endregion

    public override void OnStateEnter()
    {
        _ai.Agent.speed = _ai.Enemy.EnemyData.BaseMoveSpeed;
        _isWaiting = false;
        _waitTimer = 0f;
        _waitTime = Random.Range(1.0f, 3.0f);
        MoveToAroundSpawnPoint();
    }

    public override void OnStateUpdate()
    {
        float distance = Vector3.Distance(_ai.transform.position, _ai.PlayerTr.position);

        if (distance <= _ai.DetectionRadius)
        {
            _ai.ChangeState(EnemyAI.EEnemyState.Chase);
            return;
        }

        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitTime)
            {
                _isWaiting = false;
                _waitTimer = 0f;
                MoveToAroundSpawnPoint();
            }

            return;
        }

        if (!_ai.Agent.pathPending && (!_ai.Agent.hasPath || _ai.Agent.remainingDistance <= 0.5f))
        {
            _isWaiting = true;
            _waitTimer = 0f;
            _waitTime = Random.Range(1.0f, 3.0f);
            _ai.Agent.ResetPath();
        }
    }

    public override void OnStateExit() 
    { 
        _isWaiting = false;
        _waitTimer = 0f;
    }

    private void MoveToAroundSpawnPoint()
    {
        Vector3 randomPos = _ai.SpawnPosition + Random.insideUnitSphere * _ai.PatrolRange;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            _ai.Agent.SetDestination(hit.position);
        }
    }
}