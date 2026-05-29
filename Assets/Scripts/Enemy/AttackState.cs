using UnityEngine;

public class AttackState : FSMState
{
    public AttackState(EnemyAI ai) : base(ai) { }

    public override void OnStateEnter()
    {
        _ai.Agent.ResetPath();
    }

    public override void OnStateUpdate()
    {
        float distance = Vector3.Distance(_ai.transform.position, _ai.PlayerTr.position);

        if (distance > _ai.AttackRadius)
        {
            _ai.ChangeState(EnemyAI.EEnemyState.Chase);
            return;
        }

        Vector3 lookDir = (_ai.PlayerTr.position - _ai.transform.position).normalized;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);

            _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRot, Time.deltaTime * _ai.RotSpeed);
        }

        if (_ai.Enemy.CurrentWeapon != null)
        {
            _ai.Enemy.CurrentWeapon.TryFire(_ai.PlayerTr.position, false);

            if (_ai.Enemy.CurrentWeapon.Ammo.CurrentAmmo == 0 && !_ai.Enemy.CurrentWeapon.Ammo.IsReloading)
            {
                _ai.Enemy.CurrentWeapon.TryReload();
            }
        }
    }

    public override void OnStateExit() { }
}