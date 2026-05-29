using UnityEngine;

public class ChaseState : FSMState
{
    public ChaseState(EnemyAI ai) : base(ai) { }

    #region 내부 변수
    private float _repathTimer = 0f;
    private float _lostTimer = 0f;
    private float _chaseTimeout = 4f;
    #endregion

    public override void OnStateEnter()
    {
        _ai.Agent.speed = _ai.Enemy.EnemyData.BaseMoveSpeed;
        _lostTimer = 0f;
    }

    public override void OnStateUpdate()
    {
        float distance = Vector3.Distance(_ai.transform.position, _ai.PlayerTr.position);

        if (distance > _ai.DetectionRadius)
        {
            _lostTimer += Time.deltaTime;

            if (_lostTimer >= _chaseTimeout)
            {
                _ai.ChangeState(EnemyAI.EEnemyState.Patrol);
                return;
            }
        }

        else
        {
            _lostTimer = 0f;
        }

        if (distance <= _ai.AttackRadius)
        {
            _ai.ChangeState(EnemyAI.EEnemyState.Attack);
            return;
        }

        _repathTimer += Time.deltaTime;
        if (_repathTimer >= 0.2f)
        {
            _repathTimer = 0f;
            _ai.Agent.SetDestination(_ai.PlayerTr.position);
        }
    }

    public override void OnStateExit()
    {
        _ai.Agent.ResetPath();
    }
}