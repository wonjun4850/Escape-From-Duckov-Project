using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    #region 내부 변수
    private Weapon _weapon;

    private bool _isInit = false;
    private bool _isReloading = false;
    private int _currentAmmo = 0;
    #endregion

    #region 프로퍼티
    public bool IsReloading => _isReloading;
    public int CurrentAmmo => _currentAmmo;
    #endregion

    private IEnumerator CoReload()
    {
        if (_weapon.Audio != null)
        {
            _weapon.Audio.PlayReloadStartSound();
        }

        _isReloading = true;

        yield return new WaitForSeconds(_weapon.WeaponData.ReloadTime);

        _currentAmmo = _weapon.WeaponData.MagazineSize;

        if (_weapon.Audio != null)
        {
            _weapon.Audio.PlayReloadEndSound();
        }

        _isReloading = false;
    }

    #region 외부 호출 함수
    public void Init(Weapon weapon)
    {
        _weapon = weapon;
        _currentAmmo = 0;
        _isInit = true;
    }

    public void ConsumeAmmo()
    {
        if (_currentAmmo > 0)
        {
            _currentAmmo--;

            if (_currentAmmo == 0)
            {
                if (_weapon.Audio != null)
                {
                    _weapon.Audio.PlayAmmoEmptySound();
                }
            }
        }
    }

    public bool CanFire()
    {
        if (!_isInit)
        {
            return false;
        }

        return _currentAmmo > 0 && !_isReloading;
    }

    public void TryReload()
    {
        if (!_isInit || _isReloading)
        {
            return;
        }

        if (_currentAmmo >= _weapon.WeaponData.MagazineSize)
        {
            return;
        }

        StartCoroutine(CoReload());
    }
    #endregion
}