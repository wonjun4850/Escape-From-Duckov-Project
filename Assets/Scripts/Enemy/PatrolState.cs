using UnityEngine;

public class PatrolState : FSMState
{
    public PatrolState(EnemyAI ai) : base(ai) 
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

        if (distance <= _ai.DetectionRadius)
        {
            _ai.ChangeState(EnemyAI.EEnemyState.Chase);
        }
    }

    public override void OnStateExit() { }
}