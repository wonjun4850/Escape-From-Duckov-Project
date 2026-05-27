using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    #region 
    [Header("발사 시작지점")]
    [SerializeField] private Transform _firePoint;
    #endregion

    #region 내부 변수
    private Weapon _weapon;
    private bool _isInit = false;
    private float _lastFireTime = 0;
    #endregion

    #region 외부 호출 함수
    public void Init(Weapon weapon)
    {
        _weapon = weapon;
        _isInit = true;

        if (_firePoint == null)
        {
            Debug.LogError("firepoint 지정 필요");
        }
    }

    public void Fire(Vector3 targetPos, bool isHeadShot)
    {
        _lastFireTime = Time.time;

        if (_weapon.WeaponData.WeaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            if (_weapon.Audio != null)
            {
                _weapon.Audio.PlayMeleeSound();
            }

            return;
        }

        if (_weapon.Audio != null)
        {
            _weapon.Audio.PlayFireSound();
        }

        Vector3 target = targetPos;
        target.y = _firePoint.position.y;

        Vector3 dir = (target - _firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        float currentSpread = _weapon.FinalSpread;

        if (_weapon.IsAiming)
        {
            currentSpread *= 0.3f;
        }

        for (int i = 0; i < _weapon.WeaponData.BulletsPerShot; i++)
        {
            float randSpread = Random.Range(-currentSpread, currentSpread);

            Quaternion spread = rot * Quaternion.Euler(0, randSpread, 0);

            GameObject b = ProjectileManager.Instance.Spawn(_firePoint.position, spread);

            if (b.TryGetComponent<Projectile>(out var p))
            {
                p.Setup
                    (
                    _weapon.WeaponData.Damage,
                    _weapon.WeaponData.ProjectileLifetime,
                    _weapon.WeaponData.EffectiveRange,
                    _weapon.WeaponData.DamageOutsideEffectiveRange,
                    _weapon.WeaponData.ProjectileSpeed,
                    _firePoint.position,
                    isHeadShot,
                    _weapon.WeaponData.HeadshotMultiplier
                    );
            }
        }
    }

    public bool CanFire()
    {
        if (!_isInit)
        {
            return false;
        }

        float fireDelay = 1f / _weapon.WeaponData.FireRate;

        return Time.time >= _lastFireTime + fireDelay;
    }
    #endregion
}
