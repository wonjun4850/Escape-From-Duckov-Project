using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private WeaponItemDataSO _weaponData;
    #endregion

    #region 내부 변수
    public GameObject Owner { get; private set; }
    public bool IsAiming { get; private set; }
    public float FinalSpread { get; private set; }

    private WeaponShooter _shooter;
    private WeaponAmmo _ammo;
    private WeaponRecoil _recoil;
    private WeaponAudio _audio;
    private IngameCamera _camera;
    private bool _isPlayer;
    #endregion

    #region 프로퍼티
    public WeaponItemDataSO WeaponData => _weaponData;
    public WeaponShooter Shooter => _shooter;
    public WeaponAmmo Ammo => _ammo;
    public WeaponRecoil Recoil => _recoil;
    public WeaponAudio Audio => _audio;
    #endregion

    #region 외부 호출 함수
    public void Init(WeaponItemDataSO data, GameObject owner)
    {
        _shooter = GetComponent<WeaponShooter>();
        _ammo = GetComponent<WeaponAmmo>();
        _recoil = GetComponent<WeaponRecoil>();
        _audio = GetComponent<WeaponAudio>();
        _camera = Camera.main.GetComponent<IngameCamera>();

        _weaponData = data;
        Owner = owner;
        _isPlayer = Owner.CompareTag("Player");
        FinalSpread = _weaponData.Spread;

        if (_shooter != null)
        {
            _shooter.Init(this);
            //Debug.Log("총 슈터 초기화");
        }

        if (_ammo != null)
        {
            _ammo.Init(this);
            //Debug.Log("총 총알 초기화");
        }

        if (_recoil != null)
        {
            _recoil.Init(this);
            //Debug.Log("총 반동 초기화");
        }

        if (_audio != null)
        {
            _audio.Init(this);
            //Debug.Log("총 오디오 초기화");
        }

        if (_camera != null && _isPlayer)
        {
            _camera.ActiveWeapon(this);
            //Debug.Log("카메라 활성화");
        }

        if (!_isPlayer)
        {
            float enemySpreadOffset = owner.CompareTag("Boss") ? 0.7f : 1.3f;
            FinalSpread = _weaponData.Spread * enemySpreadOffset;
        }
    }

    public void TryFire(Vector3 target, bool isHeadShot)
    {
        if (_shooter.CanFire())
        {
            if (_ammo.CanFire())
            {
                _shooter.Fire(target, isHeadShot);
                _ammo.ConsumeAmmo();

                if (_isPlayer)
                {
                    if (_recoil != null)
                    {
                        _recoil.ApplyRecoil();
                    }

                    if (_camera != null)
                    {
                        _camera.CameraShake();
                    }

                    if (CursorManager.Instance != null)
                    {
                        CursorManager.Instance.FireCrosshairEffect();
                    }
                }
            }

            else
            {
                if (_ammo.CurrentAmmo == 0 && !_ammo.IsReloading && _weaponData.WeaponType != WeaponItemDataSO.EWeaponType.Melee)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (_audio != null)
                        {
                            _audio.PlayAmmoEmptySound();
                        }
                    }
                }
            }
        }
    }

    public void TryReload()
    {
        _ammo.TryReload();
    }

    public void SetAimState(bool isAiming)
    {
        IsAiming = isAiming;
    }
    #endregion
}