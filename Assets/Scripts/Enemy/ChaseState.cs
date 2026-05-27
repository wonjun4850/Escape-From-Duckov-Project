using UnityEngine;

public class ChaseState : FSMState
{
    private float _repathTimer = 0f;

    public ChaseState(EnemyAI ai) : base(ai) 
    { 
    
    }

    public override void OnStateEnter()
    {
        _ai.Agent.speed = _ai.Enemy.EnemyData.BaseMoveSpeed;
    }

    public override void OnStateUpdate()
    {
        if (_ai.PlayerTr == null) return;

        float distance = Vector3.Distance(_ai.transform.position, _ai.PlayerTr.position);

        if (distance > _ai.DetectionRadius)
        {
            _ai.ChangeState(EnemyAI.EEnemyState.Patrol);
            return;
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