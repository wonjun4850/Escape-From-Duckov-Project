using UnityEngine;

public abstract class FSMState
{
    protected EnemyAI _ai;

    public FSMState(EnemyAI ai)
    {
        _ai = ai;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit(); 
}