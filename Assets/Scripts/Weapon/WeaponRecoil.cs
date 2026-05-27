using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    #region 내부 변수
    private Weapon _weapon;
    private bool _isInit = false;
    public const float RECOVERY_SPEED = 5f;
    private Vector2 _currentRecoilOffset = Vector2.zero;
    #endregion

    #region 프로퍼티
    public Vector2 CurrentRecoilOffset => _currentRecoilOffset;
    #endregion

    void Update()
    {
        if (!_isInit)
        {
            return;
        }

        _currentRecoilOffset = Vector2.Lerp(_currentRecoilOffset, Vector2.zero, Time.deltaTime * RECOVERY_SPEED);
    }

    #region 외부 호출 함수
    public void Init(Weapon weapon)
    {
        _weapon = weapon;
        _isInit = true;
    }

    public void ApplyRecoil()
    {
        if (!_isInit)
        {
            return;
        }

        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(_weapon.transform.position);
        Vector2 mousePos = Input.mousePosition;
        Vector2 aimDir = (mousePos - playerScreenPos).normalized;
        Vector2 rightDir = new Vector2(aimDir.y, -aimDir.x);

        float kickbackForce = _weapon.WeaponData.RecoilY;
        float sideWobbleForce = Random.Range(-_weapon.WeaponData.RecoilX, _weapon.WeaponData.RecoilX);

        Vector2 recoilVector = (aimDir * kickbackForce) + (rightDir * sideWobbleForce);

        _currentRecoilOffset += recoilVector;
    }

    public float GetRecoverySpeed()
    {
        return RECOVERY_SPEED;
    }
    #endregion
}